using FFlow.Core;
using FFlow.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class WorkflowTests
{
    [Test]
    public async Task Workflow_ShouldBuildAndExecute()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Build();
        
        await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        Assert.Pass("Workflow executed successfully.");
    }

    [Test]
    public async Task Workflow_CancellationToken_ShouldStopExecution()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<DelayedStep>()
            .Then<TestStep>()
            .Build();
        
        var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        
        try
        {
            await workflow.RunAsync(null, cts.Token);
            Assert.Fail("Expected operation to be cancelled.");
        }
        catch (OperationCanceledException)
        {
            Assert.Pass("Workflow execution was cancelled as expected.");
        }
    }
    
    [Test]
    public async Task Workflow_WithoutExceptionHandling_ShouldCatch()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<ExceptionStep>()
            .Then<TestStep>()
            .Build();
        
        try
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
            Assert.Fail("Expected exception to be thrown.");
        }
        catch (InvalidOperationException)
        {
            Assert.Pass("Workflow execution caught the expected exception.");
        }
    }
    
    [Test]
    public async Task Workflow_WithExceptionHandling_ShouldContinue()
    {
        bool exceptionHandled = false;
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<ExceptionStep>()
            .Then<TestStep>()
            .OnAnyError((ex, ctx) =>
            {
                // Log the exception or handle it
                exceptionHandled = true;
                return Task.CompletedTask; // Continue execution
            })
            .Build();
        
        await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        if (exceptionHandled)
        {
            Assert.Pass("Workflow execution continued after handling the exception.");
        }
        else
        {
            Assert.Fail("Expected exception to be handled, but it was not.");
        }
    }
    
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
    public void Then_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow(typeof(TestService).Assembly).AddTransient<TestService>();
        
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var workflow = new FFlowBuilder(serviceProvider)
            .Then<DiStep>()
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when dependencies are injected.");
    }

    [Test]
    public void StartWith_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow(typeof(TestService).Assembly).AddTransient<TestService>();
        
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var workflow = new FFlowBuilder(serviceProvider)
            .StartWith<DiStep>()
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when dependencies are injected.");
    }
    
    [Test]
    public void Finally_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow(typeof(TestService).Assembly).AddTransient<TestService>();
        
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var workflow = new FFlowBuilder(serviceProvider)
            .Finally<DiStep>()
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when dependencies are injected.");
    }

    [Test]
    public void If_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow(typeof(TestService).Assembly).AddTransient<TestService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .If<DiStep, DiStep>(ctx => true)
            .Build(), "If<TTrue,TFalse> should inject the services");
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .If<DiStep>(ctx => true)
            .Build(), "If<TTrue> should inject the services");
        
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .If(ctx => true, () => new FFlowBuilder(serviceProvider).StartWith<DiStep>(), () => new FFlowBuilder(serviceProvider).StartWith<DiStep>())
            .Build(), "If(Builder) should inject the services");
    }

    [Test]
    public void ForEach_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow(typeof(TestService).Assembly).AddTransient<TestService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach<DiStep>(_ => []));
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach<DiStep, int>(_ => [1, 2, 3]));
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach(_ => [], () => new FFlowBuilder(serviceProvider).StartWith<DiStep>()));
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach<int>(_ => [1, 2, 3], () => new FFlowBuilder(serviceProvider).StartWith<DiStep>()));
    }
}