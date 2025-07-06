using System.Linq.Expressions;
using System.Reflection;
using FFlow.Core;

namespace FFlow;

/// <summary>
/// A builder for configuring flow steps within a workflow.
/// </summary>
public class FlowStepBuilder : ForwardingWorkflowBuilder, IConfigurableStepBuilder
{
    protected override IWorkflowBuilder Delegate { get; }
    protected readonly IFlowStep _step;
    private IFlowStep? _inputSetterStep;
    private IFlowStep? _outputSetterStep;

    private readonly List<Action<IFlowContext>> _inputSetters = new();
    private readonly List<Action<IFlowContext>> _outputWriters = new();
    private readonly IStepTemplateRegistry? _templateRegistry;

    public FlowStepBuilder(IWorkflowBuilder workflowBuilder, IFlowStep step, IStepTemplateRegistry? templateRegistry)
    {
        Delegate = workflowBuilder;
        _step = step ?? throw new ArgumentNullException(nameof(step));
        _templateRegistry = templateRegistry;
    }
    public FlowStepBuilder(IWorkflowBuilder workflowBuilder, IFlowStep step) : this(workflowBuilder, step, null)
    {
    }

    public IConfigurableStepBuilder Input<TStep>(Action<TStep> setValues) where TStep : class, IFlowStep
    {
        if (setValues is null)
        {
            throw new ArgumentNullException(nameof(setValues));
        }

        _inputSetters.Add(context =>
        {
            var step = (TStep)_step;
            setValues(step);
            foreach (var prop in typeof(TStep).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var key = Internals.BuildInputKey(step, prop.Name);
                context.SetValue(key, prop.GetValue(step)!);
            }
        });

        if (_inputSetterStep is null)
        {
            _inputSetterStep = new InputSetterStep(_inputSetters);
            InsertStepAt(GetStepCount() - 1, _inputSetterStep);
        }

        return this;
    }

    public IConfigurableStepBuilder Input<TStep, TValue>(Expression<Func<TStep, TValue>> stepProp, TValue value) where TStep : class, IFlowStep
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var setter = GetPropertySetter(stepProp);

        _inputSetters.Add(context =>
        {
            var name = GetPropertyName(stepProp);
            var key = Internals.BuildInputKey(_step, name);
            setter((TStep)_step, value);
            context.SetValue(key, value);
        });

        if (_inputSetterStep is null)
        {
            _inputSetterStep = new InputSetterStep(_inputSetters);
            InsertStepAt(GetStepCount() - 1, _inputSetterStep);
        }

        return this;
    }

    public IConfigurableStepBuilder Input<TStep, TValue>(
        Expression<Func<TStep, TValue>> stepProp,
        Func<IFlowContext, TValue> inputGetter)
        where TStep : class, IFlowStep
    {
        var setter = GetPropertySetter(stepProp);

        _inputSetters.Add(context =>
        {
            var name = GetPropertyName(stepProp);
            var key = Internals.BuildInputKey(_step, name);
            var value = inputGetter(context);
            setter((TStep)_step, value!);
            context.SetValue(key, value!);
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
            var name = GetPropertyName(stepProp);
            var key = Internals.BuildOutputKey(_step, name);
            var value = propGetter((TStep)_step);
            context.SetValue(key, value!);
            outputWriter(context, value!);
        });
        
        if(_outputSetterStep is null)
        {
            _outputSetterStep = new OutputSetterStep(_outputWriters);
            AddStep(_outputSetterStep);
        }

        return this;
    }

    public IConfigurableStepBuilder WithRetryPolicy(IRetryPolicy policy)
    {
        if (policy is null)
        {
            throw new ArgumentNullException(nameof(policy));
        }

        if (_step is IRetryableFlowStep retryableStep)
        {
            retryableStep.SetRetryPolicy(policy);
        }
        else
        {
            throw new InvalidOperationException($"The step type '{_step.GetType().Name}' does not support retry policies.");
        }

        return this;
    }

    public IConfigurableStepBuilder SkipOn(Func<IFlowContext, bool> skipOn)
    {
        if (skipOn is null)
        {
            throw new ArgumentNullException(nameof(skipOn));
        }

        if (_step is FlowStep flowStep)
        {
            flowStep.SetSkipCondition(skipOn);
        }
        else
        {
            throw new InvalidOperationException($"The step type '{_step.GetType().Name}' does not support skipping logic.");
        }

        return this;
    }

    public IConfigurableStepBuilder UseTemplate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Template name cannot be null or empty.", nameof(name));
        }

        if (_templateRegistry is null)
        {
            throw new InvalidOperationException("Template registry is not available.");
        }

        if (_templateRegistry.TryGetTemplate(_step.GetType(), name, out var configure))
        {
            configure(_step);
        }
        else
        {
            throw new KeyNotFoundException($"Template '{name}' not found in the registry.");
        }

        return this;
    }

    private static string GetPropertyName<TObj, TValue>(Expression<Func<TObj, TValue>> expr)
    {
        if (expr.Body is MemberExpression member && member.Member is PropertyInfo)
        {
            return member.Member.Name;
        }
        throw new ArgumentException("Expression must be a property access.", nameof(expr));
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