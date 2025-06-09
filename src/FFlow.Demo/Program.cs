using FFlow;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) =>
    {
        ctx.SetDotnetConfiguration(new DotnetFlowConfiguration
        {
            TargetSolution = @"/home/thiagomv/Src/DesignPatterns/DesignPatterns.sln",
            Configuration = "Debug",
            NoDependencies = false,
            NoRestore = false,
            OutputDirectory = @"/home/thiagomv/build/output",
        });
        return Task.CompletedTask;
    })
    .Then<DotnetRestoreStep>()
    .Then<DotnetBuildStep>()
    .Then((ctx, ct) =>
    {
        var output = ctx.Get<string>("DotnetBuildOutput");
        var error = ctx.Get<string>("DotnetBuildError");
        var exitCode = ctx.Get<int>("DotnetBuildExitCode");

        Console.Clear();
        Console.WriteLine("Build completed:");
        Console.WriteLine($"Exit Code: {exitCode}");
        Console.WriteLine($"Output: {output}");
        Console.WriteLine($"Error: {error}");

        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync(null, CancellationToken.None);