using FFlow.Core;

namespace FFlow;

internal class LogToConsoleStep : FlowStep
{
    public string Message { get; set; }
    
    public LogToConsoleStep(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public LogToConsoleStep()
    {
        
    }
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        Console.WriteLine(Message);
        return Task.CompletedTask;
    }
}