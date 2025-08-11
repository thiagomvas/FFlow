using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class IFlowContextTests
{
    [Test]
    public async Task GetLastOutput_ShouldReturnLastOutput()
    {
        var workflow = new FFlowBuilder()
            .StartWith<OutputStep>()
            .Input<OutputStep>(step => step.Output = "Hello World")
            .Build();
        
        var ctx = await workflow.RunAsync("");
        Assert.That(ctx.GetLastOutput<string>(), Is.EqualTo("Hello World"));
     }
    
    [Test]
    public async Task Output_ShouldBePassed_ToNextStepAsInput()
    {
        var workflow = new FFlowBuilder()
            .StartWith<OutputStep>()
                .Input<OutputStep>(step => step.Output = "Hello World")
            .Then<OutputStep>()
                .Input<OutputStep>((step, ctx) => step.Output = ctx.GetLastOutput<string>() + "!")
            .Build();
        
        var ctx = await workflow.RunAsync("");
        Assert.That(ctx.GetLastOutput<string>(), Is.EqualTo("Hello World!"));
    }

    [Test]
    public async Task SetSingleValue_ShouldOverride_WhenAlreadyExisting()
    {
        var ctx = new InMemoryFFLowContext();
        ctx.SetSingleValue<int>(1);
        Assert.That(ctx.GetSingleValue<int>(), Is.EqualTo(1), "Initial value was not set correctly");
        
        ctx.SetSingleValue<int>(2);
        Assert.That(ctx.GetSingleValue<int>(), Is.EqualTo(2), "Value was not overridden correctly");
    }
}