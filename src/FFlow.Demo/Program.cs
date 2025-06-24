using FFlow;
using FFlow.Extensions;
using FFlow.Steps.DotNet;
using FFlow.Steps.Shell;

var workflow = new FFlowBuilder()
    .RunCommand("echo 'Building FFlow...'")
    .DotnetBuild(step =>
    {
        step.ProjectOrSolution = @"/FFlow/";
        step.Configuration = "Release";
    })
    .RunCommand("echo 'Packing FFlow...'")
    .DotnetPack(step =>
    {
        step.ProjectOrSolution = @"/Src/FFlow/";
        step.Configuration = "Release";
        step.Output = "/FFlow/artifacts";
    })
    .Then((ctx, _) => Console.WriteLine($"Dotnet Pack completed with exit code {ctx.GetOutputFor<DotnetPackStep, DotnetPackResult>().ExitCode}"))
    .RunCommand("echo 'Pushing FFlow to NuGet...'")
    .DotnetNugetPush(step =>
    {
        step.PackagePathOrRoot = "/FFlow/artifacts/";
        step.Source = "https://api.nuget.org/v3/index.json";
        step.ApiKey = "API_KEY"; 
    })
    .Then((ctx, _) => Console.WriteLine($"Dotnet NuGet Push completed with exit code {ctx.GetOutputFor<DotnetNugetPushStep, DotnetNugetPushResult>().ExitCode}"))
    .OnAnyError((ctx, ct) =>
    {
        var error = ctx.GetSingleValue<Exception>();
        Console.WriteLine($"An error occurred: {error.Message}");
        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync("");