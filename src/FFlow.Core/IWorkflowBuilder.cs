namespace FFlow.Core;

public interface IWorkflowBuilder
{
    IWorkflowBuilder StartWith<TStep>() where TStep : class, IFlowStep; 
    IWorkflowBuilder StartWith(FlowAction setupAction);
    IWorkflowBuilder Then<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder Then(FlowAction setupAction);
    IWorkflowBuilder Finally<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder Finally(FlowAction setupAction);
    IWorkflowBuilder If(Func<IFlowContext, bool> condition, FlowAction then, FlowAction? otherwise = null);
    IWorkflowBuilder If<TTrue>(Func<IFlowContext, bool> condition, FlowAction? otherwise = null) where TTrue : class, IFlowStep;
    IWorkflowBuilder If<TTrue, TFalse>(Func<IFlowContext, bool> condition) 
        where TTrue : class, IFlowStep 
        where TFalse : class, IFlowStep;
    
    IWorkflowBuilder If(Func<IFlowContext, bool> condition, Func<IWorkflowBuilder> then, Func<IWorkflowBuilder>? otherwise = null);
    
    IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, FlowAction action);
    IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, FlowAction action) where TItem : class;
    IWorkflowBuilder ForEach<TStepIterator>(Func<IFlowContext, IEnumerable<object>> itemsSelector)
        where TStepIterator : class, IFlowStep;
    
    IWorkflowBuilder ForEach<TStepIterator, TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector)
        where TStepIterator : class, IFlowStep;
    
    IWorkflowBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, Func<IWorkflowBuilder> action);

    IWorkflowBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector,
        Func<IWorkflowBuilder> action);

    IWorkflowBuilder UseContext<TContext>() where TContext : class, IFlowContext;
    
    
    IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep;
    IWorkflowBuilder OnAnyError(FlowAction errorHandlerAction);
    
    IWorkflow Build();
}