using FFlow;
using FFlow.Extensions;
using FFlow.Steps.Shell;

var workflow = new FFlowBuilder()
    .StartWith((ctx, _) => 
    {
        ctx.SetValue("Greeting", "Hello from FFlow!");
        ctx.SetValue("Description", "This is a demo workflow using FFlow.");
    })
    .RunScriptRaw("echo '{context:Greeting}'\n" +
                  "echo \"foo $ENV_VAR\"\n" +
                  "echo '{context:Description}'", script => script.EnvironmentVariables = new()
        {
            {"ENV_VAR", "Some Value"}
        })
    .Build();

await workflow.RunAsync("");