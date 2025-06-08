namespace FFlow.Core;

public interface IWorkflowDefinition<TInput>
{
    IWorkflow<TInput> Build();
}