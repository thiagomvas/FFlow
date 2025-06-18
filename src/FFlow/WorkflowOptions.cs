using FFlow.Core;
    
    namespace FFlow;
    
    /// <summary>
    /// Represents configuration options for a workflow, including timeouts and step decorators.
    /// </summary>
    public class WorkflowOptions
    {
        /// <summary>
        /// Gets or sets the timeout for individual steps in the workflow.
        /// If <c>null</c>, no timeout is applied to steps.
        /// </summary>
        public TimeSpan? StepTimeout { get; set; } = null;
    
        /// <summary>
        /// Gets or sets the global timeout for the entire workflow.
        /// If <c>null</c>, no global timeout is applied.
        /// </summary>
        public TimeSpan? GlobalTimeout { get; set; } = null;
    
        /// <summary>
        /// Gets or sets the factory function used to decorate workflow steps.
        /// If <c>null</c>, no decoration is applied to steps.
        /// </summary>
        public Func<IFlowStep, IFlowStep>? StepDecoratorFactory { get; set; } = null;
        
        /// <summary>
        /// Gets or sets the event listener for workflow events.
        /// </summary>
        public IFlowEventListener? EventListener { get; set; } = null;
    
        /// <summary>
        /// Adds a decorator to the workflow steps.
        /// If a decorator already exists, the new decorator is composed with the existing one.
        /// </summary>
        /// <param name="decorator">The function to decorate workflow steps.</param>
        /// <returns>The updated <see cref="WorkflowOptions"/> instance.</returns>
        public WorkflowOptions AddStepDecorator(Func<IFlowStep, IFlowStep> decorator)
        {
            if (StepDecoratorFactory is null)
            {
                StepDecoratorFactory = decorator;
                return this;
            }
    
            var currentDecorator = StepDecoratorFactory;
            StepDecoratorFactory = step => decorator(currentDecorator(step));
            return this;
        }
        
        /// <summary>
        /// Registers an event listener for workflow events.
        /// </summary>
        /// <param name="listener">The event listener to register</param>
        /// <returns>The updated <see cref="WorkflowOptions"/> instance.</returns>
        /// <remarks>
        /// If an event listener is already registered, the new listener is added to a composite listener.
        /// There is no limit to the number of listeners that can be registered.
        /// </remarks>
        public WorkflowOptions WithEventListener(IFlowEventListener listener)
        {
            if (EventListener is null)
            {
                EventListener = listener;
            }
            else if (EventListener is CompositeFlowEventListener composite)
            {
                composite.AddListener(listener);
            }
            else
            {
                EventListener = new CompositeFlowEventListener(new[] { EventListener, listener });
            }
            return this;
        }
    }