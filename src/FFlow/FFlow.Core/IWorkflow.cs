namespace FFlow.Core;

public interface IWorkflow<in TInput>
{
    void SetGlobalErrorHandler(IFlowStep errorHandler);
    Task RunAsync(TInput input);
}