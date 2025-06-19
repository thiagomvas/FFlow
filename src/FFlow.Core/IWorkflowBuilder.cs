namespace FFlow.Core;
    
    /// <summary>
    /// Defines a builder for constructing workflows with various steps and configurations.
    /// </summary>
    public interface IWorkflowBuilder
    {

        /// <summary>
        /// Adds a step to the workflow.
        /// </summary>
        /// <param name="step">The step instance to add to the workflow.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        /// <remarks>
        /// Adding a step directly means skipping the type resolution and configuration that would normally occur.
        /// Only use this method if you have a pre-configured step instance that you want to include in the workflow.
        /// The same instance will be used each time the step is executed, so ensure it is stateless or properly managed.
        /// </remarks>
        IWorkflowBuilder AddStep(IFlowStep step);
        
        /// <summary>
        /// Starts the workflow with the specified step type.
        /// </summary>
        /// <typeparam name="TStep">The type of the step to start with.</typeparam>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IConfigurableStepBuilder StartWith<TStep>() where TStep : class, IFlowStep;
    
        /// <summary>
        /// Starts the workflow with a custom setup action.
        /// </summary>
        /// <param name="setupAction">The action to configure the starting step.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder StartWith(AsyncFlowAction setupAction);

        /// <summary>
        /// Starts the workflow with a synchronous setup action.
        /// </summary>
        /// <param name="setupAction">The action to configure the starting step.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder StartWith(SyncFlowAction setupAction);
        
        /// <summary>
        /// Adds a step to the workflow.
        /// </summary>
        /// <typeparam name="TStep">The type of the step to add.</typeparam>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IConfigurableStepBuilder Then<TStep>() where TStep : class, IFlowStep;
    
        /// <summary>
        /// Adds a step to the workflow with a custom setup action.
        /// </summary>
        /// <param name="setupAction">The action to configure the step.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder Then(AsyncFlowAction setupAction);
        
        /// <summary>
        /// Adds a step to the workflow with a synchronous setup action.
        /// </summary>
        /// <param name="setupAction">The action to configure the step.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder Then(SyncFlowAction setupAction);
    
        /// <summary>
        /// Adds a final step to the workflow.
        /// </summary>
        /// <typeparam name="TStep">The type of the final step.</typeparam>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IConfigurableStepBuilder Finally<TStep>() where TStep : class, IFlowStep;
    
        /// <summary>
        /// Adds a final step to the workflow with a custom setup action.
        /// </summary>
        /// <param name="setupAction">The action to configure the final step.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder Finally(AsyncFlowAction setupAction);
        
        /// <summary>
        /// Adds a final step to the workflow with a synchronous setup action.
        /// </summary>
        /// <param name="setupAction">The action to configure the step.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder Finally(SyncFlowAction setupAction);
    
        /// <summary>
        /// Adds a conditional step to the workflow.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="then">The action to execute if the condition is true.</param>
        /// <param name="otherwise">The optional action to execute if the condition is false.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder If(Func<IFlowContext, bool> condition, AsyncFlowAction then, AsyncFlowAction? otherwise = null);
    
        /// <summary>
        /// Adds a conditional step to the workflow with a specific step type for the true branch.
        /// </summary>
        /// <typeparam name="TTrue">The type of the step for the true branch.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="otherwise">The optional action to execute if the condition is false.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, AsyncFlowAction? otherwise = null) where TTrue : class, IFlowStep;
    
        /// <summary>
        /// Adds a conditional step to the workflow.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="then">The synchronous action to execute if the condition is true.</param>
        /// <param name="otherwise">The optional synchronous action to execute if the condition is false.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder If(Func<IFlowContext, bool> condition, SyncFlowAction then, SyncFlowAction? otherwise = null);

        /// <summary>
        /// Adds a conditional step to the workflow with a specific step type for the true branch.
        /// </summary>
        /// <typeparam name="TTrue">The type of the step for the true branch.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="otherwise">The optional synchronous action to execute if the condition is false.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, SyncFlowAction? otherwise = null) where TTrue : class, IFlowStep;

        /// <summary>
        /// Adds a conditional step to the workflow with specific step types for both true and false branches.
        /// </summary>
        /// <typeparam name="TTrue">The type of the step for the true branch.</typeparam>
        /// <typeparam name="TFalse">The type of the step for the false branch.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder If<TTrue, TFalse>(Func<IFlowContext, bool> condition)
            where TTrue : class, IFlowStep
            where TFalse : class, IFlowStep;
    
        /// <summary>
        /// Adds a conditional step to the workflow with nested workflow builders for both branches.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="then">The workflow builder for the true branch.</param>
        /// <param name="otherwise">The optional workflow builder for the false branch.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder If(Func<IFlowContext, bool> condition, Func<IWorkflowBuilder> then, Func<IWorkflowBuilder>? otherwise = null);
    
        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items.
        /// </summary>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <param name="action">The action to execute for each item.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, AsyncFlowAction action);
    
        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items of a specific type.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <param name="action">The action to execute for each item.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, AsyncFlowAction action) where TItem : class;
    
        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items.
        /// </summary>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <param name="action">The synchronous action to execute for each item.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, SyncFlowAction action);

        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items of a specific type.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <param name="action">The synchronous action to execute for each item.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, SyncFlowAction action) where TItem : class;

        
        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items with a specific step iterator type.
        /// </summary>
        /// <typeparam name="TStepIterator">The type of the step iterator.</typeparam>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach<TStepIterator>(Func<IFlowContext, IEnumerable<object>> itemsSelector)
            where TStepIterator : class, IFlowStep;
    
        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items with a specific step iterator and item type.
        /// </summary>
        /// <typeparam name="TStepIterator">The type of the step iterator.</typeparam>
        /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach<TStepIterator, TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector)
            where TStepIterator : class, IFlowStep;
    
        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items with a nested workflow builder.
        /// </summary>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <param name="action">The workflow builder to execute for each item.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, Func<IWorkflowBuilder> action);
    
        /// <summary>
        /// Adds a loop to the workflow that iterates over a collection of items of a specific type with a nested workflow builder.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
        /// <param name="itemsSelector">The function to select the collection of items.</param>
        /// <param name="action">The workflow builder to execute for each item.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector,
            Func<IWorkflowBuilder> action);
    
        /// <summary>
        /// Continues the workflow with another workflow definition.
        /// </summary>
        /// <typeparam name="TWorkflowDefinition">The type of the workflow definition to continue with.</typeparam>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder ContinueWith<TWorkflowDefinition>() where TWorkflowDefinition : class, IWorkflowDefinition;
    
        /// <summary>
        /// Adds a switch-case structure to the workflow.
        /// </summary>
        /// <param name="caseBuilder">The action to configure the switch cases.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder Switch(Action<ISwitchCaseBuilder> caseBuilder);

        /// <summary>
        /// Bifurcates the main workflow into separate subflows that run in parallel.
        /// </summary>
        /// <param name="forks">The builders for each subflow to be ran in parallel.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        /// <remarks>
        /// This creates <b>separate</b> flow contexts using <see cref="IFlowContext.Fork"/>, which means they won't share state.
        /// </remarks>
        public IWorkflowBuilder Fork(ForkStrategy strategy, params Func<IWorkflowBuilder>[] forks);
        
        /// <summary>
        /// Sets the context type for the workflow.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder UseContext<TContext>() where TContext : class, IFlowContext;
    
        /// <summary>
        /// Sets the context instance for the workflow.
        /// </summary>
        /// <param name="context">The context instance to use.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder UseContext(IFlowContext context);
    
        /// <summary>
        /// Sets the service provider for the workflow.
        /// </summary>
        /// <param name="provider">The service provider to use.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder SetProvider(IServiceProvider provider);
    
        /// <summary>
        /// Adds an error handler step to the workflow that executes on any error.
        /// </summary>
        /// <typeparam name="TStep">The type of the error handler step.</typeparam>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep;
    
        /// <summary>
        /// Adds an error handler action to the workflow that executes on any error.
        /// </summary>
        /// <param name="errorHandlerAction">The action to handle errors.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder OnAnyError(AsyncFlowAction errorHandlerAction);

        /// <summary>
        /// Adds a synchronous error handler action to the workflow that executes on any error.
        /// </summary>
        /// <param name="errorHandlerAction">The action to handle errors.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder OnAnyError(SyncFlowAction errorHandlerAction);
        
        /// <summary>
        /// Adds a delay to the workflow execution.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to delay the next step for.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder Delay(int milliseconds);
        
        /// <summary>
        /// Adds a delay to the workflow execution.
        /// </summary>
        /// <param name="delay">The time span to delay the next step for.</param>
        /// <returns>The current instance of <see cref="IWorkflowBuilder"/>.</returns>
        IWorkflowBuilder Delay(TimeSpan delay);

        IWorkflowBuilder Throw(string message);
        IWorkflowBuilder Throw<TException>(string message) where TException : Exception, new();
        IWorkflowBuilder ThrowIf(Func<IFlowContext, bool> condition, string message);
        IWorkflowBuilder ThrowIf<TException>(Func<IFlowContext, bool> condition, string message) where TException : Exception, new();

        IWorkflowBuilder InsertStepAt(int index, IFlowStep step);
        int GetStepCount();
        IWorkflowBuilder WithDecorator<TDecorator>(Func<IFlowStep, TDecorator> decoratorFactory) where TDecorator : BaseStepDecorator;
        /// <summary>
        /// Builds and returns the constructed workflow.
        /// </summary>
        /// <returns>An instance of <see cref="IWorkflow"/> representing the built workflow.</returns>
        IWorkflow Build();
    }