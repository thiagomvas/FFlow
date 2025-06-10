using FFlow;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) =>
    {
        ctx.SetDotnetPublishConfig(new()
        {
            ProjectOrSolution = @"/home/thiagomv/Src/FFlow/src/FFlow.Demo/FFlow.Demo.csproj",
        });
        return Task.CompletedTask;
    })
    .Then<DotnetPublishStep>()
    .Then((ctx, _) =>
    {
        Console.WriteLine(ctx.GetInput<DotnetPublishResult>());
        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync(null, CancellationToken.None);