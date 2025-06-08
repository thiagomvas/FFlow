namespace FFlow.Core;

public interface IWorkflow
{
    IWorkflow SetGlobalErrorHandler(IFlowStep errorHandler);
    IWorkflow SetContext(IFlowContext context);
    Task<IFlowContext> RunAsync(object input, CancellationToken cancellationToken = default);
}