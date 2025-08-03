using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;

var builder = new FFlowBuilder()
    .ForEach<int, HelloStep>(_ => [1, 2, 3], (item, step) => step.Name = $"Hello {item}");

await builder.Build().RunAsync();


