using FFlow.Core;
using FFlow.Extensions;

namespace FFlow;

/// <summary>
/// Builder for creating switch cases in a workflow.
/// </summary>
public class SwitchCaseBuilder
{
    private readonly List<SwitchCase> _cases = new List<SwitchCase>();

    internal IServiceProvider? _serviceProvider;

    /// <summary>
    /// Adds a switch case with the specified condition and allows defining its steps manually.
    /// </summary>
    /// <param name="condition">A predicate that determines when this case is executed.</param>
    /// <returns>An <see cref="IWorkflowBuilder"/> to define the steps for this case.</returns>
    public FFlowBuilder Case(Func<IFlowContext, bool> condition) => Case(string.Empty, condition);

    public FFlowBuilder Case(string Name, Func<IFlowContext, bool> condition)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));

        var builder = new FFlowBuilder(_serviceProvider);
        _cases.Add(new SwitchCase(condition, builder, Name));

        return builder;
    }

    /// <summary>
    /// Adds a switch case with the specified condition and a single starting step of type <typeparamref name="TStep"/>.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to start the case with.</typeparam>
    /// <param name="condition">A predicate that determines when this case is executed.</param>
    /// <returns>An <see cref="IWorkflowBuilder"/> to further define the case.</returns>
    public FFlowBuilder Case<TStep>(Func<IFlowContext, bool> condition) where TStep : class, IFlowStep =>
        Case<TStep>(string.Empty, condition);

    public FFlowBuilder Case<TStep>(string Name, Func<IFlowContext, bool> condition) where TStep : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));

        var builder = new FFlowBuilder(_serviceProvider);

        _cases.Add(new SwitchCase(condition, builder.Then<TStep>(), Name));

        return builder;
    }

    /// <summary>
    /// Finalizes and builds the switch step containing all defined cases.
    /// </summary>
    /// <returns>A <see cref="SwitchStep"/> representing the configured switch logic.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no cases were defined before calling this method.</exception>
    public SwitchStep Build()
    {
        if (_cases.Count == 0)
        {
            throw new InvalidOperationException("At least one case must be defined.");
        }

        return new SwitchStep(_cases);
    }
}
