using FFlow.Core;
using FFlow.Validation.Annotations;
using Workflow.Tests.Shared;

namespace FFlow.Validation.Tests;

public class AttributeTests
{
    private IFlowContext context;
    private IFlowContext empty;

    [SetUp]
    public void Setup()
    {
        empty = new TestFlowContext();
        context = new TestFlowContext();
        context.SetValue("key", "123-45-6789");
    }

    [Test]
    public async Task RequireKeyAttribute_ShouldBeApplied()
    {
        var step = new ValidatorDecorator(new RequireKeyAttributeTest());

        Assert.DoesNotThrowAsync(async () => await step.RunAsync(context),
            "Expected RunAsync to succeed when the required key is present in the context.");

        Assert.ThrowsAsync<FlowValidationException>(async () => await step.RunAsync(empty),
            "Expected FlowValidationException when the required key is missing from the context.");
    }

    [Test]
    public async Task RequireNotNullAttribute_ShouldBeApplied()
    {
        var step = new ValidatorDecorator(new RequireNotNullAttributeTest());

        Assert.DoesNotThrowAsync(async () => await step.RunAsync(context),
            "Expected RunAsync to succeed when the required key is present and not null.");

        Assert.ThrowsAsync<FlowValidationException>(async () => await step.RunAsync(empty),
            "Expected FlowValidationException when the required key is missing or is null.");

    }

    [Test]
    public async Task RequireRegexPatternAttribute_ShouldBeApplied()
    {
        var step = new ValidatorDecorator(new RequireRegexAttributeTest());

        Assert.DoesNotThrowAsync(async () => await step.RunAsync(context),
            "Expected RunAsync to succeed when the key matches the regex pattern.");

        Assert.ThrowsAsync<FlowValidationException>(async () => await step.RunAsync(empty),
            "Expected FlowValidationException when the key is missing from the context.");

        empty.SetValue("key", "1234567890");

        Assert.ThrowsAsync<FlowValidationException>(async () => await step.RunAsync(empty),
            "Expected FlowValidationException when the key is present but does not match the regex pattern.");
    }
}
