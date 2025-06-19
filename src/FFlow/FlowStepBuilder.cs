using System.Linq.Expressions;
using System.Reflection;
using FFlow.Core;

namespace FFlow;

public class FlowStepBuilder : ForwardingWorkflowBuilder, IConfigurableStepBuilder
{
    protected override IWorkflowBuilder Delegate { get; }
    public IWorkflowBuilder Builder => Delegate;
    private readonly IFlowStep _step;
    private IFlowStep? _inputSetterStep;

    private readonly List<Action<IFlowContext>> _inputSetters = new();
    private readonly List<Action<IFlowContext>> _outputWriters = new();
    
    public FlowStepBuilder(IWorkflowBuilder workflowBuilder, IFlowStep step)
    {
        Delegate = workflowBuilder;
        _step = step ?? throw new ArgumentNullException(nameof(step));
    }

    public IConfigurableStepBuilder Input<TStep, TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Func<IFlowContext, TValue> inputGetter)
        where TStep : class, IFlowStep
    {
        var setter = GetPropertySetter(stepProp);

        _inputSetters.Add(context =>
        {
            var value = inputGetter(context);
            setter((TStep)_step, value!);
        });
        
        if(_inputSetterStep is null)
        {
            _inputSetterStep = new InputSetterStep(_inputSetters);
            InsertStepAt(GetStepCount() - 1, _inputSetterStep);
        }

        return this;
    }
    
    public IConfigurableStepBuilder Output<TStep, TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Action<IFlowContext, TValue> outputWriter)
        where TStep : class, IFlowStep
    {
        var propGetter = stepProp.Compile();

        _outputWriters.Add(context =>
        {
            var value = propGetter((TStep)_step);
            outputWriter(context, value!);
        });

        return this;
    }

    private static Action<TObj, TValue> GetPropertySetter<TObj, TValue>(Expression<Func<TObj, TValue>> propExpr)
    {
        if (propExpr.Body is MemberExpression member && member.Member is PropertyInfo propInfo)
        {
            var objParam = Expression.Parameter(typeof(TObj), "obj");
            var valueParam = Expression.Parameter(typeof(TValue), "value");
            var setExpr = Expression.Lambda<Action<TObj, TValue>>(
                Expression.Assign(Expression.Property(objParam, propInfo), valueParam),
                objParam,
                valueParam);
            return setExpr.Compile();
        }
        throw new ArgumentException("Expression must be a property access.", nameof(propExpr));
    }
}