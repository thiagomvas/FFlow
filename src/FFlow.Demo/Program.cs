using FFlow;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) =>
    {
        ctx.SetDotnetRunConfig(new()
        {
            Project = @"/home/thiagomv/Src/DesignPatterns/DesignPatterns/DesignPatterns.csproj",
        });
        return Task.CompletedTask;
    })
    .Then<DotnetRunStep>()
    .Then((ctx, _) =>
    {
        Console.WriteLine(ctx.GetInput<DotnetRunResult>());
        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync(null, CancellationToken.None);