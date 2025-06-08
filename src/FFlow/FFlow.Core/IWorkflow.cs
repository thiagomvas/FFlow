namespace FFlow.Core;

public interface IWorkflow
{
    void SetGlobalErrorHandler(IFlowStep errorHandler);
    Task RunAsync(object input, CancellationToken cancellationToken = default);
}