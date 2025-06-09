using FFlow;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) =>
    {
        ctx.SetDotnetConfiguration(new DotnetFlowConfiguration
        {
            TargetProject = @"/home/thiagomv/Src/FFlow/tests/FFlow.Tests/FFlow.Tests.csproj",
        });
        return Task.CompletedTask;
    })
    .Then<DotnetRestoreStep>()
    .Then<DotnetBuildStep>()
    .Then<DotnetTestStep>()
    .Then((ctx, ct) =>
    {
        var output = ctx.Get<string>("DotnetTestOutput");
        var error = ctx.Get<string>("DotnetTestError");
        var exitCode = ctx.Get<int>("DotnetTestExitCode");

        Console.WriteLine("Build completed:");
        Console.WriteLine($"Exit Code: {exitCode}");
        Console.WriteLine($"Output: {output}");
        Console.WriteLine($"Error: {error}");

        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync(null, CancellationToken.None);