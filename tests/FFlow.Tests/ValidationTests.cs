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
}