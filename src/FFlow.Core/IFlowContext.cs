namespace FFlow.Core;

/// <summary>
/// Represents the context for a workflow, providing methods to manage input and key-value pairs.
/// </summary>
public interface IFlowContext
{
    void SetInput<TStep, TInput>(TInput input) where TStep : class, IFlowStep
        where TInput : class;
    void SetOutput<TStep, TOutput>(TOutput output) where TStep : class, IFlowStep
        where TOutput : class;
    TInput? GetInputFor<TStep, TInput>() where TStep : class, IFlowStep
        where TInput : class;
    TOutput? GetOutputFor<TStep, TOutput>() where TStep : class, IFlowStep
        where TOutput : class;
    T? GetValue<T>(string key, T defaultValue = null) where T : class;
    void SetValue<T>(string key, T value);
    T? GetSingleValue<T>() where T : class;
    void SetSingleValue<T>(T value);
}