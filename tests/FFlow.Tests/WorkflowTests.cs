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
        
        var decoratedCounter = ctx.Get<int>("decorated_counter");
        Assert.That(decoratedCounter, Is.EqualTo(1), "The step decorator should have incremented the counter.");
    }
}