using FFlow.Core;
using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class DefaultStepTests
{
    
    [Test]
    public async Task Workflow_ShouldHandleForEach()
    {
        int[] items = { 1, 2, 3 };
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) =>
            {
                ctx.SetInput(items);
                return Task.CompletedTask;
            })
            .ForEach(ctx => ctx.GetInput<IEnumerable<int>>(), () =>
            {
                return new FFlowBuilder().StartWith<TestStep>();
            })
            .Build();
        
        var ctx = await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        int counter = ctx.Get<int>("counter");
        Assert.That(counter, Is.EqualTo(items.Length), $"ForEach should iterate over all items, instead iterated over {counter}.");
    }

    [Test]
    public async Task Workflow_ShouldHandleIfStep_WhenTrue()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .If<TestStep, ExceptionStep>(ctx => true)
            .Build();
        
        await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        Assert.Pass("Workflow executed If step successfully.");
    }
    
    [Test]
    public async Task Workflow_ShouldHandleIfStep_WhenFalse()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .If<ExceptionStep, TestStep>(ctx => false)
            .Build();
        
        await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        Assert.Pass("Workflow executed If step successfully executing the false branch.");
    }

    [Test]
    public async Task Workflow_ShouldHandleSwitch()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Switch(builder =>
            {
                builder.Case(_ => 1 == 1).Then<TestStep>();
                builder.Case(_ => 1 == 1).Then<ExceptionStep>();
                builder.Case(_ => 1 == 4).Then<ExceptionStep>();
            })
            .Build();

        await workflow.RunAsync(null);
    }

    [Test]
    public async Task Fork_WhenWaitForAll_ShouldWaitBeforeContinuing()
    {
        var messages = new List<string>();
        var workflow = new FFlowBuilder()
            .StartWith((_, _) => messages.Add("Starting"))
            .Fork(ForkStrategy.WaitForAll, 
                () => new FFlowBuilder()
                    .Then((_, _) => messages.Add("1")),
                () => new FFlowBuilder()
                    .Then((_, _) => messages.Add("2")))
            .Then((_, _) => messages.Add("All"))
            .Build();
        
        await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        // Check if 1 and 2 are present before "All"
        Assert.That(messages, Does.Contain("1"), "Message '1' should be present in the messages.");
        Assert.That(messages, Does.Contain("2"), "Message '2' should be present in the messages.");
        Assert.Multiple(() =>
        {
            Assert.That(messages.IndexOf("1"), Is.LessThan(messages.IndexOf("All")), "Message '1' should appear before 'All'.");
            Assert.That(messages.IndexOf("2"), Is.LessThan(messages.IndexOf("All")), "Message '2' should appear before 'All'.");
            Assert.That(messages, Does.Contain("All"), "Message 'All' should be present in the messages.");
        });
    }
    
    [Test]
    public async Task Fork_WhenFireAndForget_ShouldNotWaitBeforeContinuing()
    {
        var messages = new List<string>();
        var workflow = new FFlowBuilder()
            .StartWith((_, _) => messages.Add("Starting"))
            .Fork(ForkStrategy.FireAndForget, 
                () => new FFlowBuilder()
                    .Then((_, _) => messages.Add("1")),
                () => new FFlowBuilder()
                    .Then((_, _) => messages.Add("2")))
            .Then((_, _) => messages.Add("All"))
            .Build();
        
        await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        
        Assert.That(messages, Does.Contain("1"), "Message '1' should be present in the messages.");
        Assert.That(messages, Does.Contain("2"), "Message '2' should be present in the messages.");
        Assert.That(messages, Does.Contain("All"), "Message 'All' should be present in the messages.");
    }

    [Test]
    public async Task Fork_WhenThrowingSingle_ShouldHandle()
    {


        var workflow = new FFlowBuilder()
            .StartWith((_, _) => Task.Run(() => Console.WriteLine("Starting")))
            .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder()
                    .Then((_, _) => Console.WriteLine("Task 1")),
                () => new FFlowBuilder()
                    .Then((_, _) => throw new Exception("Task 2 threw an exception"))
                    .Then((_, _) => Console.WriteLine("Task 2")),
                () => new FFlowBuilder()
                    .Then((_, _) => Console.WriteLine("Task 3")))
            .OnAnyError((ctx, ct) =>
            {
                var ex = ctx.Get<Exception>("Exception");
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex, Is.InstanceOf<AggregateException>());
                
                // Check the inners
                var aggregateException = ex as AggregateException;
                Assert.That(aggregateException?.InnerExceptions, Has.Count.EqualTo(1));
                Assert.That(aggregateException?.InnerExceptions[0], Is.InstanceOf<Exception>());
                Assert.That(aggregateException?.InnerExceptions[0].Message, Is.EqualTo("Task 2 threw an exception"));
                
                return Task.CompletedTask;
            })
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when handling a single forked step that throws an exception.");
    }
    
    [Test]
    public async Task Fork_WhenThrowingMultiple_ShouldHandle()
    {
        var workflow = new FFlowBuilder()
            .StartWith((_, _) => Task.Run(() => Console.WriteLine("Starting")))
            .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder()
                    .Then((_, _) => Console.WriteLine("Task 1")),
                () => new FFlowBuilder()
                    .Then((_, _) => throw new Exception("Task 2 threw an exception"))
                    .Then((_, _) => Console.WriteLine("Task 2")),
                () => new FFlowBuilder()
                    .Then((_, _) => throw new Exception("Task 3 threw an exception"))
                    .Then((_, _) => Console.WriteLine("Task 3")))
            .OnAnyError((ctx, ct) =>
            {
                var ex = ctx.Get<Exception>("Exception");
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex, Is.InstanceOf<AggregateException>());
                
                // Check the inners
                var aggregateException = ex as AggregateException;
                Assert.That(aggregateException?.InnerExceptions, Has.Count.EqualTo(2));
                Assert.That(aggregateException?.InnerExceptions[0], Is.InstanceOf<Exception>());
                Assert.That(aggregateException?.InnerExceptions[1], Is.InstanceOf<Exception>());
                
                return Task.CompletedTask;
            })
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when handling multiple forked steps that throw exceptions.");
    }
}