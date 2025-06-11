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
    
    
    
}