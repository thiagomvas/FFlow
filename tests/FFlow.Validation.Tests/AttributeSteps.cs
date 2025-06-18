using FFlow.Core;
using FFlow.Validation.Annotations;

namespace FFlow.Validation.Tests;

[RequireKey("key")]
public class RequireKeyAttributeTest : DummyStep;

[RequireNotNull("key")]
public class RequireNotNullAttributeTest : DummyStep;

[RequireRegex("key", @"^\d{3}-\d{2}-\d{4}$")]
public class RequireRegexAttributeTest : DummyStep;

public class DummyStep : IFlowStep
{
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}