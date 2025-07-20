using System.Linq.Expressions;
using FFlow.Core;

namespace FFlow;

/// <summary>
/// A strongly-typed builder for configuring a specific type of flow step within a workflow.
/// </summary>
/// <typeparam name="TStep">The type of step being configured.</typeparam>
public class FlowStepBuilder<TStep> : FlowStepBuilder
    where TStep : class, IFlowStep
{
    public FlowStepBuilder(IWorkflowBuilder workflowBuilder, TStep step, IStepTemplateRegistry? templateRegistry)
        : base(workflowBuilder, step, templateRegistry)
    {
        
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="FlowStepBuilder{TStep}"/> class.
    /// </summary>
    /// <param name="workflowBuilder">The parent workflow builder.</param>
    /// <param name="step">The step being configured.</param>
    public FlowStepBuilder(IWorkflowBuilder workflowBuilder, TStep step) : base(workflowBuilder, step)
    {
    }

    /// <summary>
    /// Configures input values for the step using a delegate.
    /// </summary>
    /// <param name="setValues">An action to set the input values for the step.</param>
    /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
    public IConfigurableStepBuilder Input(
        Action<TStep> setValues)
    {
        return Input<TStep>(setValues);
    }

    /// <summary>
    /// Configures a specific input property of the step with a given value.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <param name="stepProp">An expression representing the property to configure.</param>
    /// <param name="value">The value to set for the property.</param>
    /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
    public IConfigurableStepBuilder Input<TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        TValue value)
    {
        return Input<TStep, TValue>(stepProp, value);
    }

    /// <summary>
    /// Configures a specific input property of the step using a function to retrieve the value from the flow context.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <param name="stepProp">An expression representing the property to configure.</param>
    /// <param name="inputGetter">A function to retrieve the value from the flow context.</param>
    /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
    public IConfigurableStepBuilder Input<TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Func<IFlowContext, TValue> inputGetter)
    {
        return Input<TStep, TValue>(stepProp, inputGetter);
    }

    /// <summary>
    /// Configures an output property of the step to write its value to the flow context.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <param name="stepProp">An expression representing the property to configure.</param>
    /// <param name="outputWriter">An action to write the value to the flow context.</param>
    /// <returns>The current instance of <see cref="IConfigurableStepBuilder"/> for chaining.</returns>
    public IConfigurableStepBuilder Output<TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Action<IFlowContext, TValue> outputWriter)
    {
        return Output<TStep, TValue>(stepProp, outputWriter);
    }
    
    public IConfigurableStepBuilder Configure(
        Action<TStep> configureAction)
    {
        if (configureAction is null)
        {
            throw new ArgumentNullException(nameof(configureAction));
        }

        configureAction((TStep)_step);
        return this;
    }
}
