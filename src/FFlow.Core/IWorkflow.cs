namespace FFlow.Core;
    
    /// <summary>
    /// Represents a workflow that can be configured and executed.
    /// </summary>
    public interface IWorkflow
    {
        /// <summary>
        /// Sets a global error handler for the workflow.
        /// </summary>
        /// <param name="errorHandler">The error handler step to execute when an error occurs.</param>
        /// <returns>The current instance of <see cref="IWorkflow"/>.</returns>
        IWorkflow SetGlobalErrorHandler(IFlowStep errorHandler);
    
        /// <summary>
        /// Sets the context for the workflow.
        /// </summary>
        /// <param name="context">The context instance to use during workflow execution.</param>
        /// <returns>The current instance of <see cref="IWorkflow"/>.</returns>
        IWorkflow SetContext(IFlowContext context);
    
        /// <summary>
        /// Executes the workflow asynchronously with the specified input and cancellation token.
        /// </summary>
        /// <param name="input">The input object to pass to the workflow.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation, containing the resulting <see cref="IFlowContext"/>.</returns>
        Task<IFlowContext> RunAsync(object input, CancellationToken cancellationToken = default);
    }