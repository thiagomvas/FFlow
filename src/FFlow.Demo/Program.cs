using System.Reflection;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Extensions.Microsoft.DependencyInjection;
using FFlow.Observability.Extensions;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;
using FFlow.Steps.Shell;
using Microsoft.Extensions.DependencyInjection;

var workflow = new FFlowBuilder()
    .StartWith((ctx, _) => 
    {
        ctx.SetValue("Greeting", "Hello from FFlow!");
        ctx.SetValue("Description", "This is a demo workflow using FFlow.");
    })
    .RunScriptRaw("echo '{context:Greeting}'\n" +
                  "echo 'This is a raw script execution step.'\n" +
                  "echo '{context:Description}'")
    .Build();

await workflow.RunAsync("");