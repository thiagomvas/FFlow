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
    .RunCommand("echo 'Hello, World!'")
    .Build();

await workflow.RunAsync("");