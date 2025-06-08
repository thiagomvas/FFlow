using FFlow;
using FFlow.Demo;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddTransient<ServiceA>();
services.AddTransient<HelloStep>();
var flow = new FFlowBuilder<object?>(services.BuildServiceProvider())
    .StartWith<HelloStep>()
    .Build();

await flow.RunAsync(null);

    public class ServiceA
    {
        public string Name { get; set; } = "ServiceA";
    }