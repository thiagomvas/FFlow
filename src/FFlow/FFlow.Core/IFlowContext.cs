namespace FFlow.Core;

public interface IFlowContext
{
    T Get<T>(string key);
    void Set<T>(string key, T value);
}