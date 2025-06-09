namespace FFlow.Core;

public interface ISwitchCaseBuilder
{
    IWorkflowBuilder Case(Func<IFlowContext, bool> condition);
    IWorkflowBuilder Case<TStep>(Func<IFlowContext, bool> condition) where TStep : class, IFlowStep;
}