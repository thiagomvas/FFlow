using FFlow;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.DotNet;

var registry = new StepTemplateRegistry();
var flow = new FFlowBuilder(null, registry)
    .DotnetBuild("/home/thiagomv/Src/FFlow/FFlow.sln")
    .LogToConsole(ctx => $".NET Build completed with exit code: {ctx.GetDotnetBuildOutput().ExitCode}")
    .DotnetTest("/home/thiagomv/Src/FFlow/FFlow.sln", step => step.NoBuild = true)
    .Finally((ctx, _) => 
    {
        var output = ctx.GetDotnetTestOutput();
        Console.WriteLine("Test Results:");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Passed: {output.Passed}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed: {output.Failed}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Skipped: {output.Skipped}");
        Console.ResetColor();
    })
    .Build();

var ctx = await flow.RunAsync();

