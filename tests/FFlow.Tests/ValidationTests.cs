namespace FFlow.Tests;

public class ValidationTests
{
    [Test]
    public async Task CheckForKeyStep_ShouldThrow_IfKeyNotFound()
    {
        var workflow = new FFlowBuilder()
            .CheckForKey("non_existent_key")
            .Build();

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should throw KeyNotFoundException when the key does not exist in the context.");
    }
    
    [Test]
    public async Task CheckForKeyStep_ShouldNotThrow_IfKeyExists()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.Set("existing_key", "value"))
            .CheckForKey("existing_key")
            .Build();

        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should not throw when the key exists in the context.");
    }
    
    public async Task RegexPatternStep_ShouldThrow_IfValueDoesNotMatchPattern()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.Set("test_key", "invalid_value"))
            .CheckForRegexPattern("test_key", @"^\d{3}-\d{2}-\d{4}$") // Example pattern: 123-45-6789
            .Build();

        Assert.ThrowsAsync<FormatException>(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should throw FormatException when the value does not match the regex pattern.");
    }
    
    [Test]
    public async Task RegexPatternStep_ShouldNotThrow_IfValueMatchesPattern()
    {
        var workflow = new FFlowBuilder()
            .StartWith((ctx, ct) => ctx.Set("test_key", "123-45-6789")) // Example pattern: 123-45-6789
            .CheckForRegexPattern("test_key", @"^\d{3}-\d{2}-\d{4}$")
            .Build();

        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Should not throw when the value matches the regex pattern.");
    }
}