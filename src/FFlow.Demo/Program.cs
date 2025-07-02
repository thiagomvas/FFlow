using FFlow;
using FFlow.Demo;
using FFlow.Steps.DotNet;

var registry = new StepTemplateRegistry();
registry.OverrideDefaults<HelloStep>(step => step.Name = "Default Name");
registry.RegisterTemplate<HelloStep>("john", step => step.Name = "John Doe");

var flow = new FFlowBuilder(null, registry)
    .StartWith<HelloStep>()
    .Input<HelloStep>(step => step.Name = "Jane Doe")
    .UseTemplate("john")
    .Build();

var ctx = await flow.RunAsync("", CancellationToken.None);

