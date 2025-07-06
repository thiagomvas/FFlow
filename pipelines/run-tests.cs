#:package FFlow@1.0.0
#:package FFlow.Steps.DotNet@1.0.0
  
using FFlow;
using FFlow.Extensions;
using FFlow.Steps.DotNet;

await new FFlowBuilder()
    .WithPipelineLogger()
    .DotnetBuild(".")
    .DotnetTest(".", step => step.NoBuild = true)
    .ThrowIf<Exception>(ctx => ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>().Failed > 0, "Tests have failed")
    .Then((ctx, _) => {
        var output = ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n\nTests passed: {output.Passed}, Failed: {output.Failed}, Skipped: {output.Skipped}");
    })
    .Build()
    .RunAsync();

