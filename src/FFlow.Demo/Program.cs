using FFlow;
using FFlow.Extensions;
using FFlow.Steps.Shell;

var workflow = new FFlowBuilder()
    .RunCommand("echo", step => {
        step.Arguments = "Hello, World!";
        step.OutputHandler = Console.WriteLine;
    })
    .Build();

await workflow.RunAsync("");