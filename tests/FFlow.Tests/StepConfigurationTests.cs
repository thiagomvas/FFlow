using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class StepConfigurationTests
{
    [Test]
    public async Task ConfigureStepInputs_ShouldWork_WithPropSetterAndInputGetter()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, _) => ctx.Set("increment", 2))
            .Then<TestStep>()
            .Input<TestStep, int>(step => step.Increment,
                ctx => ctx.Get<int>("increment"))
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.Get<int>("counter"), Is.EqualTo(2));
    }
    
    [Test]
    public async Task ConfigureStepInputs_ShouldWork_WithPropSetterAndValue()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, _) => ctx.Set("increment", 3))
            .Then<TestStep>()
            .Input<TestStep, int>(step => step.Increment, 5)
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.Get<int>("counter"), Is.EqualTo(5));
    }
    
    [Test]
    public async Task ConfigureStepInputs_ShouldWork_WithActionSetter()
    {
        var workflow = new FFlowBuilder()
            .Then<TestStep>()
            .Input<TestStep>(step => step.Increment = 4)
            .Build();
        
        var ctx = await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);
        Assert.That(ctx.Get<int>("counter"), Is.EqualTo(4));
    }
}