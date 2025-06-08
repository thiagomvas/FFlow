using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddFFlow(typeof(HelloStep).Assembly);

var serviceProvider = services.BuildServiceProvider();
var workflow = serviceProvider.GetRequiredService<HelloWorkflow>();
await workflow.Build().RunAsync(null);