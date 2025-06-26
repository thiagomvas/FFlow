namespace FFlow.Core;
    
    /// <summary>
    /// Represents a workflow that can be configured and executed.
    /// </summary>
    public interface IWorkflow
    {
        public IWorkflowMetadataStore MetadataStore { get; }
        /// <summary>
        /// Sets a global error handler for the workflow.
        /// </summary>
        /// <param name="errorHandler">The error handler step to execute when an error occurs.</param>
        /// <returns>The current instance of <see cref="IWorkflow"/>.</returns>
        IWorkflow SetGlobalErrorHandler(IFlowStep errorHandler);
    
        /// <summary>
        /// Gets the current context of the workflow.
        /// </summary>
        /// <returns>The context of the current workflow execution.</returns>
        IFlowContext GetContext();
        
        /// <summary>
        /// Sets the context for the workflow.
        /// </summary>
        /// <param name="context">The context instance to use during workflow execution.</param>
        /// <returns>The current instance of <see cref="IWorkflow"/>.</returns>
        IWorkflow SetContext(IFlowContext context);
        
        /// <summary>
        /// Sets a finalizer step for the workflow.
        /// </summary>
        /// <param name="finalizer">The finalizer step that will be run at the end of the execution.</param>
        /// <returns>The current instance of <see cref="IWorkflow"/>.</returns>
        /// <remarks>
        /// The finalizer step is executed after all other steps have completed, regardless of whether they succeeded or failed.
        /// </remarks>
        IWorkflow SetFinalizer(IFlowStep finalizer);
        /// <summary>
        /// Executes the workflow asynchronously with the specified input and cancellation token.
        /// </summary>
        /// <param name="input">The input object to pass to the workflow.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation, containing the resulting <see cref="IFlowContext"/>.</returns>
        Task<IFlowContext> RunAsync(object input, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes the compensation logic asynchronously to undo or handle rollback actions.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous compensation operation, returning the resulting <see cref="IFlowContext"/>.</returns>
        Task<IFlowContext> CompensateAsync(CancellationToken cancellationToken = default);
    }