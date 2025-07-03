#:package FFlow@1.0.0
#:package FFlow.Steps.DotNet@1.0.0
  
using FFlow;
using FFlow.Extensions;
using FFlow.Steps.DotNet;

await new FFlowBuilder()
  .WithPipelineLogger()
  .StartWith((ctx, _) => Console.WriteLine("Initializing Pipeline..."))
  .DotnetBuild("..")
  .DotnetTest("..", step => step.NoBuild = true)
  .ThrowIf<Exception>(ctx => ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>().Failed > 0, "Tests have failed")
  .DotnetPack("..", step => { step.Configuration = "Release", step.Output = "nupkgs" })
  .DotnetNugetPush("nupkgs")
  .Build()
  .RunAsync();