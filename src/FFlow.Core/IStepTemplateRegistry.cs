namespace FFlow.Core;

/// <summary>
/// Defines a registry for managing step templates in workflows.
/// </summary>
public interface IStepTemplateRegistry
{
    /// <summary>
    /// Registers a step template with a specified name and configuration action.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to register.</typeparam>
    /// <param name="name">The name of the template.</param>
    /// <param name="configure">The action to configure the step.</param>
    void RegisterTemplate<TStep>(string name, Action<TStep> configure) where TStep : IFlowStep;

    /// <summary>
    /// Registers a step template with a specified name and configuration action using the step type.
    /// </summary>
    /// <param name="stepType">The type of the step to register.</param>
    /// <param name="name">The name of the template.</param>
    /// <param name="configure">The action to configure the step.</param>
    void RegisterTemplate(Type stepType, string name, Action<IFlowStep> configure);

    /// <summary>
    /// Retrieves a registered step template by name.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to retrieve.</typeparam>
    /// <param name="name">The name of the template.</param>
    /// <returns>The configuration action for the step.</returns>
    Action<TStep> GetTemplate<TStep>(string name) where TStep : IFlowStep;

    /// <summary>
    /// Attempts to retrieve a registered step template by name.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to retrieve.</typeparam>
    /// <param name="name">The name of the template.</param>
    /// <param name="configure">The configuration action for the step, if found.</param>
    /// <returns><c>true</c> if the template is found; otherwise, <c>false</c>.</returns>
    bool TryGetTemplate<TStep>(string name, out Action<TStep> configure) where TStep : IFlowStep;

    /// <summary>
    /// Attempts to retrieve a registered step template by name using the step type.
    /// </summary>
    /// <param name="stepType">The type of the step to retrieve.</param>
    /// <param name="name">The name of the template.</param>
    /// <param name="configure">The configuration action for the step, if found.</param>
    /// <returns><c>true</c> if the template is found; otherwise, <c>false</c>.</returns>
    bool TryGetTemplate(Type stepType, string name, out Action<IFlowStep> configure);

    /// <summary>
    /// Overrides the default configuration for a step type.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to override defaults for.</typeparam>
    /// <param name="configure">The action to configure the step.</param>
    void OverrideDefaults<TStep>(Action<TStep> configure) where TStep : IFlowStep;

    /// <summary>
    /// Attempts to retrieve overridden default configuration for a step type.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to retrieve defaults for.</typeparam>
    /// <param name="configure">The overridden default configuration action, if found.</param>
    /// <returns><c>true</c> if overridden defaults are found; otherwise, <c>false</c>.</returns>
    bool TryGetOverridenDefaults<TStep>(out Action<IFlowStep> configure) where TStep : IFlowStep;

    /// <summary>
    /// Attempts to retrieve overridden default configuration for a step type using the step type.
    /// </summary>
    /// <param name="type">The type of the step to retrieve defaults for.</param>
    /// <param name="configure">The overridden default configuration action, if found.</param>
    /// <returns><c>true</c> if overridden defaults are found; otherwise, <c>false</c>.</returns>
    bool TryGetOverridenDefaults(Type type, out Action<IFlowStep> configure);
}