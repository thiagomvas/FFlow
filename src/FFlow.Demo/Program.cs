using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions.Microsoft.DependencyInjection;
using FFlow.Steps.DotNet;
using Microsoft.Extensions.DependencyInjection;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) =>
    {
        ctx.SetTargetSolution(@"/home/thiagomv/Src/FFlow/FFlow.sln");
        ctx.SetTargetProject(@"/home/thiagomv/Src/FFlow/src/FFlow.Demo/FFlow.Demo.csproj");
        return Task.CompletedTask;
    })
    .Then<DotnetRestoreStep>()
    .Then((ctx, ct) =>
    {
        var output = ctx.Get<string>("DotnetRestoreOutput");
        var error = ctx.Get<string>("DotnetRestoreError");
        var exitCode = ctx.Get<int>("DotnetRestoreExitCode");

        Console.WriteLine($"Output: {output}");
        Console.WriteLine($"Error: {error}");
        Console.WriteLine($"Exit Code: {exitCode}");

        return Task.CompletedTask;
    })
    .Build();
    
await workflow.RunAsync(null, CancellationToken.None);