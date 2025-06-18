using System.Collections.Concurrent;
using System.Reflection;
using FFlow.Core;
using FFlow.Validation.Annotations;

namespace FFlow.Validation;

/// <summary>
/// Decorator that applies validation steps to a workflow step.
/// </summary>
/// <remarks>
/// The validators are applied through attributes on the step class.
/// </remarks>
public class ValidatorDecorator : BaseStepDecorator
{
    private static readonly ConcurrentDictionary<Type, List<Func<IFlowStep>>> _validatorsCache = new();
    private readonly List<IFlowStep> _validationSteps;
    public ValidatorDecorator(IFlowStep innerStep) : base(innerStep)
    {
        var stepType = innerStep.GetType();
            
        var factories = _validatorsCache.GetOrAdd(stepType, BuildFactories);

        _validationSteps = factories.Select(factory => factory()).ToList();
    }

    private static List<Func<IFlowStep>> BuildFactories(Type stepType)
    {
        var attrs = stepType.GetCustomAttributes<BaseFlowValidationAttribute>(inherit: false);

        var factories = attrs.Select<BaseFlowValidationAttribute, Func<IFlowStep>>(attr => attr.CreateValidationStep).ToList();

        return factories;
    }

    public override async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        foreach (var validationStep in _validationSteps)
        {
            await validationStep.RunAsync(context, cancellationToken);
        }
        await base.RunAsync(context, cancellationToken);
    }
}