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
    public async Task Workflow_ShouldTimeout_AfterGlobalTimeout()
    {
        var workflow = new FFlowBuilder()
            .WithOptions(options => options.GlobalTimeout = TimeSpan.FromMilliseconds(200))
            .Delay(120)
            .Delay(120)
            .Then((_, _) => Assert.Fail())
            .Build();

        try
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
            Assert.Fail("Expected operation to timeout.");
        }
        catch (OperationCanceledException)
        {
            Assert.Pass("Workflow execution timed out as expected.");
        }
    }

    [Test]
    public async Task Workflow_ShouldTimeout_AfterStepTimeout()
    {
        var workflow = new FFlowBuilder()
            .WithOptions(options => options.StepTimeout = TimeSpan.FromMilliseconds(200))
            .Delay(300)
            .Then((_, _) => Assert.Fail())
            .Build();

        try
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
            Assert.Fail("Expected operation to timeout.");
        }
        catch (OperationCanceledException)
        {
            Assert.Pass("Workflow execution timed out as expected.");
        }
    }

    [Test]
    public async Task Workflow_ShouldUseDecorators()
    {
        var workflow = new FFlowBuilder()
            .WithOptions(options => options.StepDecoratorFactory = step => new TestStepDecorator(step))
            .StartWith<TestStep>()
            .Build();

        var ctx = await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);

        var decoratedCounter = ctx.GetValue<int>("decorated_counter");
        Assert.That(decoratedCounter, Is.EqualTo(1), "The step decorator should have incremented the counter.");
    }

    [Test]
    public async Task WorkflowListener_ShouldBeInvoked()
    {
        var listener = new TestFlowEventListener();
        var workflow = new FFlowBuilder()
            .WithOptions(options => options.EventListener = listener)
            .StartWith<TestStep>()
            .Then<TestStep>()
            .Build();

        await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);

        Assert.That(listener.WorkflowStartedCount, Is.EqualTo(1), "Workflow started event should be invoked.");
        Assert.That(listener.StepStartedCount, Is.EqualTo(2), "Step started event should be invoked.");
        Assert.That(listener.WorkflowCompletedCount, Is.EqualTo(1), "Workflow completed event should be invoked.");
        Assert.That(listener.StepCompletedCount, Is.EqualTo(2), "Step completed event should be invoked.");
    }

    [Test]
    public async Task WorkflowListener_ShouldCountWithErrorsCorrectly()
    {
        var listener = new TestFlowEventListener();
        var workflow = new FFlowBuilder()
            .WithOptions(options => options.EventListener = listener)
            .StartWith<TestStep>()
            .Throw<InvalidOperationException>("Simulated error")
            .Then<TestStep>()
            .OnAnyError((_, _) => { })
            .Build();

        await workflow.RunAsync(null);
        Assert.That(listener.WorkflowStartedCount, Is.EqualTo(1), "Workflow started event should be invoked.");
        Assert.That(listener.StepStartedCount, Is.EqualTo(2), "Step started event should be invoked.");
        Assert.That(listener.WorkflowCompletedCount, Is.EqualTo(1), "Workflow completed event should not be invoked.");
        Assert.That(listener.StepCompletedCount, Is.EqualTo(1),
            "Step completed event should be invoked for the first step only.");
        Assert.That(listener.StepFailedCount, Is.EqualTo(1),
            "Step errored event should be invoked for the step that threw an exception.");
        Assert.That(listener.WorkflowFailedCount, Is.EqualTo(1),
            "Workflow errored event should be invoked since we handled the error.");
    }

    [Test]
    public async Task Finalizer_ShouldRun()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<TestStep>()
            .Finally((ctx, ct) =>
            {
                ctx.SetValue("finalized", true);
            })
            .Build();

        var ctx = await workflow.RunAsync(null);
        Assert.That(ctx.GetValue<bool>("finalized"), Is.True,
            "The finalizer step should have run and set the 'finalized' context value to true.");
    }

    [Test]
    public async Task Finalizer_WhenThrown_ShouldRun()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Throw<Exception>("Simulated error")
            .Finally((ctx, ct) =>
            {
                ctx.SetValue("finalized", true);
            })
            .OnAnyError((_, _) => { })
            .Build();

        await workflow.RunAsync(null);
        var ctx = await workflow.RunAsync(null);
        Assert.That(ctx.GetValue<bool>("finalized"), Is.True,
            "The finalizer step should have run even after an exception was thrown.");
    }
    
    [Test]
    public async Task Step_ShouldSkip_WhenConditionIsTrue()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<ExceptionStep>().SkipOn(ctx => ctx.GetValue<int>("counter") > 0)
            .Then<TestStep>()
            .Build();

        Assert.DoesNotThrowAsync(async () =>
        {
            var ctx = await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
            Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(2), "Counter should be incremented.");
        });
    }
}