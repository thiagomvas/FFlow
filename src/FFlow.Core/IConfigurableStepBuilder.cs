using System.Linq.Expressions;
    
    namespace FFlow.Core;
    
    /// <summary>
    /// Represents a builder for configuring steps in a workflow.
    /// </summary>
    public interface IConfigurableStepBuilder : IWorkflowBuilder
    {
        /// <summary>
        /// Configures input values for a step using a delegate.
        /// </summary>
        /// <typeparam name="TStep">The type of the step.</typeparam>
        /// <param name="setValues">An action to set the input values for the step.</param>
        /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
        public IConfigurableStepBuilder Input<TStep>(
            Action<TStep> setValues)
            where TStep : class, IFlowStep;
    
        /// <summary>
        /// Configures a specific input property of a step with a given value.
        /// </summary>
        /// <typeparam name="TStep">The type of the step.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="stepProp">An expression representing the property to configure.</param>
        /// <param name="value">The value to set for the property.</param>
        /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
        public IConfigurableStepBuilder Input<TStep, TValue>(
            Expression<Func<TStep, TValue>> stepProp,
            TValue value)
            where TStep : class, IFlowStep;
    
        /// <summary>
        /// Configures a specific input property of a step using a function to retrieve the value from the flow context.
        /// </summary>
        /// <typeparam name="TStep">The type of the step.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="stepProp">An expression representing the property to configure.</param>
        /// <param name="inputGetter">A function to retrieve the value from the flow context.</param>
        /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
        public IConfigurableStepBuilder Input<TStep, TValue>(
            Expression<Func<TStep, TValue>> stepProp,
            Func<IFlowContext, TValue> inputGetter)
            where TStep : class, IFlowStep;
    
        /// <summary>
        /// Configures an output property of a step to write its value to the flow context.
        /// </summary>
        /// <typeparam name="TStep">The type of the step.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="stepProp">An expression representing the property to configure.</param>
        /// <param name="outputWriter">An action to write the value to the flow context.</param>
        /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
        public IConfigurableStepBuilder Output<TStep, TValue>(
            Expression<Func<TStep, TValue>> stepProp,
            Action<IFlowContext, TValue> outputWriter)
            where TStep : class, IFlowStep;
    
        /// <summary>
        /// Configures a retry policy for the step.
        /// </summary>
        /// <param name="policy">The retry policy to apply.</param>
        /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
        public IConfigurableStepBuilder WithRetryPolicy(IRetryPolicy policy);
        
        public IConfigurableStepBuilder SkipOn(Func<IFlowContext, bool> skipOn);
    }