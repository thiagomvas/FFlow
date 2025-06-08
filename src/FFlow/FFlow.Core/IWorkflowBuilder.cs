namespace FFlow.Core;

public interface IWorkflowBuilder
{
    IWorkflowBuilder StartWith<TStep>() where TStep : class, IFlowStep; 
    IWorkflowBuilder StartWith(Func<IFlowContext, Task> setupAction);
    IWorkflowBuilder Then<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder Then(Func<IFlowContext, Task> setupAction);
    IWorkflowBuilder Finally<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder Finally(Func<IFlowContext, Task> setupAction);
    
    IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder OnAnyError(Func<IFlowContext, Task> errorHandlerAction);
    
    IWorkflow Build();
}