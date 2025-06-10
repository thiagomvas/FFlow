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
        ctx.SetDotnetTestConfig(new()
        {
            ProjectOrSolution = @"/home/thiagomv/Src/FFlow/tests/FFlow.Tests/FFlow.Tests.csproj",
            NoBuild = true,
            NoRestore = true,
        });
        return Task.CompletedTask;
    })
    .Then<DotnetRestoreStep>()
    .Then((_, _) => Task.Run(() =>Console.WriteLine("Restored project or solution.")))
    .Then<DotnetBuildStep>()
    .Then((_, _) => Task.Run(() => Console.WriteLine("Built project or solution.")))
    .Then<DotnetTestStep>()
    .Then((ctx, _) =>
    {
        Console.WriteLine(ctx.GetInput<DotnetTestResult>());
        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync(null, CancellationToken.None);