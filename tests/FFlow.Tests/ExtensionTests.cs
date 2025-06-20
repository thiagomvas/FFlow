using FFlow.Extensions;
using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class ExtensionTests
{
    [Test]
    public void LoadEnvironmentVariables_ShouldLoadVariables()
    {
        var context = new TestFlowContext();
        Environment.SetEnvironmentVariable("TEST_VAR", "123");

        context.LoadEnvironmentVariables();

        Assert.That(context.Get<string>("TEST_VAR"), Is.EqualTo("123"));
        
        // Cleanup
        Environment.SetEnvironmentVariable("TEST_VAR", null);
    }
}