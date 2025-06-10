using FFlow;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) =>
    {
        ctx.SetDotnetRestoreConfig(new()
        {
            ProjectOrSolution = @"/home/thiagomv/Src/FFlow/src/FFlow.Demo/FFlow.Demo.csproj"
        });
        ctx.SetDotnetBuildConfig(new ()
        {
            ProjectOrSolution = @"/home/thiagomv/Src/FFlow/src/FFlow.Demo/FFlow.Demo.csproj"
        });
        return Task.CompletedTask;
    })
    .Then<DotnetRestoreStep>()
    .Then<DotnetBuildStep>()
    .Build();

await workflow.RunAsync(null, CancellationToken.None);