namespace FFlow.Core;

public interface IStepTemplateRegistry
{
    void RegisterTemplate<TStep>(string name, Action<TStep> configure) where TStep : IFlowStep;
    void RegisterTemplate(Type stepType, string name, Action<IFlowStep> configure);
    Action<TStep> GetTemplate<TStep>(string name) where TStep : IFlowStep;
    bool TryGetTemplate<TStep>(string name, out Action<TStep> configure) where TStep : IFlowStep;
    bool TryGetTemplate(Type stepType, string name, out Action<IFlowStep> configure);
    void OverrideDefaults<TStep>(Action<TStep> configure) where TStep : IFlowStep;
    bool TryGetOverridenDefaults<TStep>(out Action<IFlowStep> configure) where TStep : IFlowStep;
    bool TryGetOverridenDefaults(Type type, out Action<IFlowStep> configure);
}