using FFlow.Core;

namespace FFlow;

public static class FFlowBuilderStepConfigurationExtensions
{
    public static WorkflowBuilderBase Input<TStep>(this WorkflowBuilderBase builder,
        Action<TStep, IFlowContext> configure)
        where TStep : class, IFlowStep
    {
        var lastStep = builder.Steps[^1];
        if (lastStep is null) throw new InvalidOperationException("No steps have been added to the flow.");
        if (lastStep is not TStep step)
            throw new InvalidOperationException($"Last step is not of type {typeof(TStep).Name}.");

        if (step is FlowStep fs and TStep ts)
        {
            fs.OnBeforeRun += (flowStep, ctx) => configure(flowStep as TStep, ctx);
            return builder;
        }

        int beforeStepIndex = builder.Steps.Count - 1;
        var beforeStep = builder.Steps[beforeStepIndex];

        if (beforeStep is InputSetterStep setter)
        {
            setter._inputSetters.Add((ctx) => configure(step, ctx));
        }
        else
        {
            var inputSetter = new InputSetterStep(new List<Action<IFlowContext>> { (ctx) => configure(step, ctx) });
            builder.InsertStepAt(beforeStepIndex, inputSetter);
        }

        return builder;
    }

    public static WorkflowBuilderBase Input<TStep>(this WorkflowBuilderBase builder, Action<TStep> configure)
        where TStep : class, IFlowStep
        => builder.Input<TStep>((step, _) => configure.Invoke(step));

    public static WorkflowBuilderBase Output<TStep>(this WorkflowBuilderBase builder,
        Action<TStep, IFlowContext> configure)
        where TStep : class, IFlowStep
    {
        var lastStep = builder.Steps[^1];
        if (lastStep is null) throw new InvalidOperationException("No steps have been added to the flow.");
        if (lastStep is not TStep step)
            throw new InvalidOperationException($"Last step is not of type {typeof(TStep).Name}.");

        if (step is FlowStep fs and TStep ts)
        {
            fs.OnBeforeRun += (flowStep, ctx) => configure(flowStep as TStep, ctx);
            return builder;
        }

        int beforeStepIndex = builder.Steps.Count - 1;
        var beforeStep = builder.Steps[beforeStepIndex];

        if (beforeStep is OutputSetterStep setter)
        {
            setter._outputWriters.Add((ctx) => configure(step, ctx));
        }
        else
        {
            var inputSetter = new OutputSetterStep(new List<Action<IFlowContext>> { (ctx) => configure(step, ctx) });
            builder.InsertStepAt(beforeStepIndex, inputSetter);
        }

        return builder;
    }

    public static WorkflowBuilderBase Output<TStep>(this WorkflowBuilderBase builder, Action<TStep> configure)
        where TStep : class, IFlowStep
        => builder.Output<TStep>((step, _) => configure.Invoke(step));
    
    
    public static WorkflowBuilderBase UseTemplate(this WorkflowBuilderBase builder, string name)
    {
        var lastStep = builder.Steps[^1];
        if(builder.StepTemplateRegistry.TryGetTemplate(lastStep.GetType(), name, out var configure))
            configure.Invoke(lastStep);
        
        return builder;
    }

    public static WorkflowBuilderBase SkipOn(this WorkflowBuilderBase builder, Func<IFlowContext, bool> skipOn)
    {
        var lastStep = builder.Steps[^1];
        if (lastStep is ISkippableStep skippable)
        {
            skippable.SetSkipCondition(skipOn);
        }
        else
        {
            throw new InvalidOperationException($"The step type '{lastStep.GetType().Name}' does not support skipping logic.");
        }

        return builder;
        
    }

    public static WorkflowBuilderBase WithRetryPolicy(this WorkflowBuilderBase builder, IRetryPolicy policy)
    {
        var lastStep = builder.Steps[^1];
        if (lastStep is IRetryableFlowStep retryable)
        {
            retryable.SetRetryPolicy(policy);
        }
        else
        {
            throw new InvalidOperationException($"The step type '{lastStep.GetType().Name}' does not support retry policies.");
        }
        return builder;
    }
}