namespace FFlow.Core;

public interface IFlowStep
{
    Task RunAsync(IFlowContext context);
}