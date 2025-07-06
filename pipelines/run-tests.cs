#:package FFlow@1.0.0
#:package FFlow.Steps.DotNet@1.0.0
  
using FFlow;
using FFlow.Extensions;
using FFlow.Steps.DotNet;


const string solutionPath = ".";

await new FFlowBuilder()
    .Then((ctx, _) => Console.WriteLine("Starting .NET build..."))
    .DotnetBuild(solutionPath, step => step.Verbosity = "quiet")
    .Then((ctx, _) => Console.WriteLine($".NET Build completed with exit code: {ctx.GetOutputFor<DotnetBuildStep, DotnetBuildResult>().ExitCode}"))
    
    .Then((ctx, _) => Console.WriteLine("Starting .NET tests (no build)..."))
    .DotnetTest(solutionPath, step =>
    {
        step.NoBuild = true;
        step.Verbosity = "quiet";
    })
    
    .Then((ctx, _) =>
    {
        var output = ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>();

        Console.WriteLine("\n=============================");
        Console.WriteLine("       ✅ Test Summary       ");
        Console.WriteLine("=============================\n");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  ✔ Passed : {output.Passed}");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ✘ Failed : {output.Failed}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  ⚠ Skipped: {output.Skipped}");

        Console.ResetColor();
        Console.WriteLine("\n=============================\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Powered by FFlow!\n\n");
        Console.ResetColor();
    })
    
    .Then((ctx, _) => Console.WriteLine("Pipeline execution finished successfully!"))
    .Build()
    .RunAsync();

