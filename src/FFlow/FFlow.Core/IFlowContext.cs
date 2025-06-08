namespace FFlow.Core;

public interface IFlowContext
{
    TInput GetInput<TInput>();
    void SetInput<TInput>(TInput input);
    T Get<T>(string key);
    void Set<T>(string key, T value);
}