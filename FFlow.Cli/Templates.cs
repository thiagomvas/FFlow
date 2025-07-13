namespace FFlow.Cli;

public static class Templates
{
    public static readonly Template Basic = new Template("basic", "A basic template with minimal setup.",
        """
        #:package FFlow@1.*
        
        using FFlow;
        
        await new FFlowBuilder()
            .Build()
            .RunAsync();
        """);
    
    public const string EmptyBuilder = """
        await new FFlowBuilder()
            .Build()
            .RunAsync();
        """;
    
    public const string ExampleStep = """
        public class HelloWorldStep : FlowStep
        {
            protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
            {
                Console.WriteLine("Hello, World!");
                return Task.CompletedTask;
            }
        }
        """;
}