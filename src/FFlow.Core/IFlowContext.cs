namespace FFlow.Core;

/// <summary>
/// Represents the context for a workflow, providing methods to manage input and key-value pairs.
/// </summary>
public interface IFlowContext
{
    /// <summary>
    /// Sets the input for the specified step type.
    /// </summary>
    /// <typeparam name="TStep">The type of the workflow step.</typeparam>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input value to set.</param>
    void SetInputFor<TStep, TInput>(TInput input) where TStep : class, IFlowStep;

    /// <summary>
    /// Sets the input for the specified step instance.
    /// </summary>
    /// <typeparam name="TStep">The type of the workflow step.</typeparam>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="step">The workflow step instance.</param>
    /// <param name="input">The input value to set.</param>
    void SetInputFor<TStep, TInput>(TStep step, TInput input) where TStep : class, IFlowStep;

    /// <summary>
    /// Sets the output for the specified step type.
    /// </summary>
    /// <typeparam name="TStep">The type of the workflow step.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="output">The output value to set.</param>
    void SetOutputFor<TStep, TOutput>(TOutput output) where TStep : class, IFlowStep;

    /// <summary>
    /// Sets the output for the specified step instance.
    /// </summary>
    /// <typeparam name="TStep">The type of the workflow step.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="step">The workflow step instance.</param>
    /// <param name="output">The output value to set.</param>
    void SetOutputFor<TStep, TOutput>(TStep step, TOutput output) where TStep : class, IFlowStep;

    /// <summary>
    /// Gets the input for the specified step type.
    /// </summary>
    /// <typeparam name="TStep">The type of the workflow step.</typeparam>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <returns>The input value if set; otherwise, null.</returns>
    TInput? GetInputFor<TStep, TInput>() where TStep : class, IFlowStep;

    /// <summary>
    /// Gets the output for the specified step type.
    /// </summary>
    /// <typeparam name="TStep">The type of the workflow step.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <returns>The output value if set; otherwise, null.</returns>
    TOutput? GetOutputFor<TStep, TOutput>() where TStep : class, IFlowStep;

    /// <summary>
    /// Gets the output for the specified step type.
    /// </summary>
    /// <param name="stepType">The type of step to get the output for</param>
    /// <returns>The output for said step, if present</returns>
    object? GetOutputFor(Type stepType);

    /// <summary>
    /// Gets a value associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="defaultValue">The default value to return if the key is not found.</param>
    /// <returns>The value if found; otherwise, the default value.</returns>
    T? GetValue<T>(string key, T defaultValue = default);

    /// <summary>
    /// Sets a value associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The key of the value to set.</param>
    /// <param name="value">The value to set.</param>
    void SetValue<T>(string key, T value);

    /// <summary>
    /// Gets a single stored value of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <returns>The stored value if set; otherwise, null.</returns>
    T? GetSingleValue<T>();

    /// <summary>
    /// Sets a single stored value of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to set.</param>
    void SetSingleValue<T>(T value);

    /// <summary>
    /// Gets the last input stored of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <returns>The last input if available; otherwise, null.</returns>
    T? GetLastInput<T>();

    /// <summary>
    /// Gets the last output stored of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the output.</typeparam>
    /// <returns>The last output if available; otherwise, null.</returns>
    T? GetLastOutput<T>();

    /// <summary>
    /// Creates a forked copy of the current flow context.
    /// </summary>
    /// <returns>A new <see cref="IFlowContext"/> instance representing the forked context.</returns>
    IFlowContext Fork();
}