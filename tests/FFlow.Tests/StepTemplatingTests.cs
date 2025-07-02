
using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class StepTemplatingTests
{
    [Test]
    public async Task OverridenDefaults_ShouldApply()
    {
        var registry = new StepTemplateRegistry();
        registry.OverrideDefaults<TestStep>(step => step.Increment = 5);

        var flow = new FFlowBuilder(null, registry)
            .StartWith<TestStep>()
            .Build();
        
        var ctx = await flow.RunAsync("", CancellationToken.None);
        
        Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(5));
    }
    
    [Test]
    public async Task RegisterTemplate_ShouldApply()
    {
        var registry = new StepTemplateRegistry();
        registry.RegisterTemplate<TestStep>("test", step => step.Increment = 10);

        var flow = new FFlowBuilder(null, registry)
            .StartWith<TestStep>()
            .UseTemplate("test")
            .Build();
        
        var ctx = await flow.RunAsync("", CancellationToken.None);
        
        Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(10));
    }
    
    [Test]
    public async Task Input_ShouldOverride_Templates()
    {
        var registry = new StepTemplateRegistry();
        registry.RegisterTemplate<TestStep>("test", step => step.Increment = 10);

        var flow = new FFlowBuilder(null, registry)
            .StartWith<TestStep>()
            .Input<TestStep>(step => step.Increment = 20)
            .UseTemplate("test")
            .Build();
        
        var ctx = await flow.RunAsync("", CancellationToken.None);
        
        Assert.That(ctx.GetValue<int>("counter"), Is.EqualTo(20));
    }
}