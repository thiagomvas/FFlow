namespace FFlow.Core;

public interface IWorkflow<in TInput> 
{
    Task RunAsync(TInput input);
}