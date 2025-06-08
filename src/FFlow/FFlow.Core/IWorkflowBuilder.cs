namespace FFlow.Core;

public interface IWorkflowBuilder
{
    IWorkflowBuilder StartWith<TStep>() where TStep : class, IFlowStep; 
    IWorkflowBuilder StartWith(Func<IFlowContext, Task> setupAction);
    IWorkflowBuilder Then<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder Then(Func<IFlowContext, Task> setupAction);
    IWorkflowBuilder Finally<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder Finally(Func<IFlowContext, Task> setupAction);
    IWorkflowBuilder If(Func<IFlowContext, bool> condition, Func<IFlowContext, Task> then, Func<IFlowContext, Task>? otherwise = null);
    IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, Func<IFlowContext, Task>? otherwise = null) where TTrue : class, IFlowStep;
    IWorkflowBuilder If<TTrue, TFalse>(Func<IFlowContext, bool> condition) 
        where TTrue : class, IFlowStep 
        where TFalse : class, IFlowStep;
    
    IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder OnAnyError(Func<IFlowContext, Task> errorHandlerAction);
    
    IWorkflow Build();
}