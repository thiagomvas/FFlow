namespace FFlow.Core;

/// <summary>
/// Represents the context for a workflow, providing methods to manage input and key-value pairs.
/// </summary>
public interface IFlowContext
{
    void SetInputFor<TStep, TInput>(TInput input) where TStep : class, IFlowStep;
    void SetInputFor<TStep, TInput>(TStep step, TInput input) where TStep : class, IFlowStep;
    void SetOutputFor<TStep, TOutput>(TOutput output) where TStep : class, IFlowStep;
    void SetOutputFor<TStep, TOutput>(TStep step, TOutput output) where TStep : class, IFlowStep;
    TInput? GetInputFor<TStep, TInput>() where TStep : class, IFlowStep;
    TOutput? GetOutputFor<TStep, TOutput>() where TStep : class, IFlowStep;
    T? GetValue<T>(string key, T defaultValue = default);
    void SetValue<T>(string key, T value);
    T? GetSingleValue<T>();
    void SetSingleValue<T>(T value);

    T? GetLastInput<T>();
    T? GetLastOutput<T>();

    IFlowContext Fork();
}