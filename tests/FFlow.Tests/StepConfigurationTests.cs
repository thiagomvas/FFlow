using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class StepConfigurationTests
{
    [Test]
    public async Task ConfigureStepInputs_ShouldWork_WithPropSetterAndInputGetter()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, _) => ctx.SetValue("increment", 2))
            .Then<TestStep>()
            .Input<TestStep>((step, ctx) => step.Increment = ctx.GetValue<int>("increment"))
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(2));
    }

    [Test]
    public async Task ConfigureStepInputs_ShouldWork_WithMultiple()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, _) => ctx.SetValue("increment", 2))
            .Then<TestStep>()
            .Input<TestStep>((step, ctx) => step.Increment = ctx.GetValue<int>("increment"))
            .Input<TestStep>((step, ctx) => step.Increment = 5)
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(5));
        
    }
    
    [Test]
    public async Task ConfigureStepInputs_ShouldWork_WithPropSetterAndValue()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, _) => ctx.SetValue("increment", 3))
            .Then<TestStep>()
            .Input<TestStep>((step, ctx) => step.Increment = 5)
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(5));
    }
    
    [Test]
    public async Task ConfigureStepInputs_ShouldWork_WithActionSetter()
    {
        var workflow = new FFlowBuilder()
            .Then<TestStep>()
            .Input<TestStep>(step => step.Increment = 4)
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(4));
    }
    
    [Test]
    public async Task WithRetryPolicy_ShouldCorrectlyApplyPolicy()
    {
        var retryCount = 0;
        var workflow = new FFlowBuilder()
            .StartWith((ctx, _) =>
            {
                var counter = ctx.GetValue<int>("retry_count");
                ctx.SetValue("retry_count", counter + 1);
                throw new InvalidOperationException("Test exception");
            })
            .WithRetryPolicy(RetryPolicies.FixedDelay(3, TimeSpan.FromMilliseconds(100)))
            .Then((ctx, _) =>
            {
                ctx.SetValue("failed", true);
            })
            .OnAnyError((_, _) => {})
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.GetValue<int>("retry_count"), Is.EqualTo(3));
        Assert.That(ctx.GetValue<bool>("failed"), Is.False, "The workflow should have cancelled execution.");
    }
}