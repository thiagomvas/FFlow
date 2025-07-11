namespace FFlow.Validation.Tests;

public class ValidationStepTests
{
    
    [Test]
    public async Task CheckForKeyStep_ShouldThrow_IfKeyNotFound()
    {
        var workflow = new FFlowBuilder()
            .RequireKey("non_existent_key")
            .Build();

        Assert.ThrowsAsync<FlowValidationException>(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should throw KeyNotFoundException when the key does not exist in the context.");
    }
    
    [Test]
    public async Task CheckForKeyStep_ShouldNotThrow_IfKeyExists()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("existing_key", "value"))
            .RequireKey("existing_key")
            .Build();

        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should not throw when the key exists in the context.");
    }
    
    [Test]
    public async Task RegexPatternStep_ShouldThrow_IfValueDoesNotMatchPattern()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("test_key", "invalid_value"))
            .RequireRegex("test_key", @"^\d{3}-\d{2}-\d{4}$") // Example pattern: 123-45-6789
            .Build();

        Assert.ThrowsAsync<FlowValidationException>(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should throw FormatException when the value does not match the regex pattern.");
    }
    
    [Test]
    public async Task RegexPatternStep_ShouldNotThrow_IfValueMatchesPattern()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("test_key", "123-45-6789")) // Example pattern: 123-45-6789
            .RequireRegex("test_key", @"^\d{3}-\d{2}-\d{4}$")
            .Build();

        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should not throw when the value matches the regex pattern.");
    }
    
    [Test]
    public async Task NotNullStep_ShouldThrow_IfValueIsNull()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue<object?>("test_key", null))
            .RequireNotNull("test_key")
            .Build();

        Assert.ThrowsAsync<FlowValidationException>(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should throw ArgumentNullException when the value is null.");
    }
    
    [Test]
    public async Task NotNullStep_ShouldNotThrow_IfValueIsNotNull()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("test_key", "value"))
            .RequireNotNull("test_key")
            .Build();

        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should not throw when the value is not null.");
    }
    
    [Test]
    public async Task NotEmptyStep_ShouldThrow_IfValueIsEmpty()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("test_key", string.Empty))
            .RequireNotEmpty("test_key")
            .Build();

        Assert.ThrowsAsync<FlowValidationException>(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should throw ArgumentNullException when the value is empty.");
        
        var workflowWithCollection = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("test_collection", new List<string>()))
            .RequireNotEmpty("test_collection")
            .Build();
        
        Assert.ThrowsAsync<FlowValidationException>(async () =>
        {
            await workflowWithCollection.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should throw ArgumentNullException when the collection is empty.");
    }
    
    [Test]
    public async Task NotEmptyStep_ShouldNotThrow_IfValueIsNotEmpty()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("test_key", "value"))
            .RequireNotEmpty("test_key")
            .Build();

        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should not throw when the value is not empty.");
        
        var workflowWithCollection = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.SetValue("test_collection", new List<string> { "item1" }))
            .RequireNotEmpty("test_collection")
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflowWithCollection.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should not throw when the collection is not empty.");
    }
}