#:package FFlow@1.0.0
#:package FFlow.Steps.DotNet@1.0.0
  
using FFlow;
using FFlow.Extensions;
using FFlow.Steps.DotNet;

await new FFlowBuilder()
    .WithPipelineLogger()
    .StartWith((ctx, _) => ctx.LoadEnvironmentVariables())
    .DotnetBuild(".")
    .DotnetTest(".", step => step.NoBuild = true)
    .ThrowIf<Exception>(ctx => ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>().Failed > 0, "Tests have failed")
    .DotnetPublish(".", step => step.Configuration = "Release")
    .DotnetPack(".", step =>
    {
        step.Configuration = "Release";
        step.Output = "nupkgs";
    })
    .DotnetNugetPush("nupkgs/")
    .Input<DotnetNugetPushStep, string>(step => step.ApiKey,  ctx => ctx.GetValue<string>("NUGET_API_KEY"))
    .Build()
    .RunAsync();

