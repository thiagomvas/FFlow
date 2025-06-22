using FFlow.Core;
using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class CompensationTests
{
    [Test]
    public async Task CompensableStep_ShouldNotCompensate_WhenNoError()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<CompensableStep>()
            .OnAnyError((ctx, _) => {})
            .Build();
        
        var ctx = await workflow.RunAsync("");
        
        Assert.That(ctx.GetOutputFor<CompensableStep, string>(), Is.EqualTo("executed"), "Compensation executed unexpectedly.");
    }
    [Test]
    public async Task Workflow_ShouldCompensate_WhenThrown()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Then<CompensableStep>()
            .Throw<Exception>("Simulated")
            .OnAnyError((ctx, _) => {})
            .Build();
        
        var ctx = await workflow.RunAsync("");
        
        Assert.That(ctx.GetOutputFor<CompensableStep, bool>(), Is.False, "Compensation did not execute as expected.");
    }

    [Test]
    public async Task Fork_ShouldCompensate_OnlyErroredBranches()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder().StartWith<CompensableStep>().Throw<Exception>("Simulated"))
            .Then((ctx, _) => ctx.SetValue("success", true))
            .OnAnyError((ctx, _) => {})
            .Build();
        
        var ctx = await workflow.RunAsync("");
        
        Assert.That(ctx.GetValue<bool>("success"), Is.True, "Success value was not set as expected.");
    }
    
    [Test]
    public async Task Compensation_Executes_WhenConditionIsTrue()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .If<CompensableStep, TestStep>(_ => true)
            .Throw<Exception>("Simulated")
            .OnAnyError((ctx, _) => { })
            .Build();

        var ctx = await workflow.RunAsync("");

        Assert.That(ctx.GetOutputFor<CompensableStep, string>(), Is.EqualTo("compensated"), "Compensation did not execute for 'true' path as expected.");
    }

    [Test]
    public async Task Compensation_Executes_WhenConditionIsFalse()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .If<TestStep, CompensableStep>(_ => false)
            .Throw<Exception>("Simulated")
            .OnAnyError((ctx, _) => { })
            .Build();

        var ctx = await workflow.RunAsync("");

        Assert.That(ctx.GetOutputFor<CompensableStep, string>(), Is.EqualTo("compensated"), "Compensation did not execute for 'false' path as expected.");
    }

    [Test]
    public async Task Compensation_DoesNotExecute_WhenConditionIsFalse_PathTrue()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .If<CompensableStep, TestStep>(_ => false)
            .Throw<Exception>("Simulated")
            .OnAnyError((ctx, _) => { })
            .Build();

        var ctx = await workflow.RunAsync("");

        // Compensation on false path should NOT execute
        Assert.That(ctx.GetOutputFor<CompensableStep, string>(), Is.Null.Or.EqualTo(string.Empty), "False path step incorrectly compensated or executed when condition is true.");
    }

    [Test]
    public async Task Compensation_DoesNotExecute_WhenConditionIsTrue_PathFalse()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .If<TestStep, CompensableStep>(_ => true)
            .Throw<Exception>("Simulated")
            .OnAnyError((ctx, _) => { })
            .Build();

        var ctx = await workflow.RunAsync("");

        // Compensation on true path should NOT execute
        Assert.That(ctx.GetOutputFor<CompensableStep, string>(), Is.Null.Or.EqualTo(string.Empty), "True path step incorrectly compensated or executed when condition is false.");
    }
    
    [Test]
    public async Task Compensation_Executes_WhenCompensableStepInSwitchCase()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Switch(switchBuilder =>
            {
                switchBuilder.Case<CompensableStep>(_ => true);
                switchBuilder.Case<TestStep>(_ => false);
            })
            .Throw<Exception>("Simulated")
            .OnAnyError((ctx, _) => { })
            .Build();

        var ctx = await workflow.RunAsync("");

        Assert.That(ctx.GetOutputFor<CompensableStep, string>(), Is.EqualTo("compensated"), "Compensation did not execute for compensable step in switch case as expected.");
    }
    
    [Test]
    public async Task Compensation_DoesNotExecute_WhenNonCompensableStepInSwitchCase()
    {
        var workflow = new FFlowBuilder()
            .StartWith<TestStep>()
            .Switch(switchBuilder =>
            {
                switchBuilder.Case<TestStep>(_ => true);
                switchBuilder.Case<CompensableStep>(_ => false);
            })
            .Throw<Exception>("Simulated")
            .OnAnyError((ctx, _) => { })
            .Build();

        var ctx = await workflow.RunAsync("");

        // Non-compensable step should not execute compensation
        Assert.That(ctx.GetOutputFor<CompensableStep, string>(), Is.Null.Or.EqualTo(string.Empty), "Non-compensable step incorrectly compensated or executed when condition is true.");
    }
}