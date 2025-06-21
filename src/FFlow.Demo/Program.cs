using System.Reflection;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Observability.Listeners;


var workflow = new HelloWorkflow().Build();
    
await workflow.RunAsync("", CancellationToken.None);