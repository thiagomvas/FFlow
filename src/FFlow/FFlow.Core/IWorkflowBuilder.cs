namespace FFlow.Core;

public interface IWorkflowBuilder<in TInput>
{
    IWorkflowBuilder<TInput> StartWith<TStep>() where TStep : IFlowStep; 
    IWorkflowBuilder<TInput> StartWith(Func<IFlowContext, Task> setupAction);
    IWorkflowBuilder<TInput> Then<TStep>() where TStep : IFlowStep;
    IWorkflowBuilder<TInput> Then(Func<IFlowContext, Task> setupAction);
    IWorkflowBuilder<TInput> Finally<TStep>() where TStep : IFlowStep;
    IWorkflowBuilder<TInput> Finally(Func<IFlowContext, Task> setupAction);
    
    IWorkflow<TInput> Build();
}