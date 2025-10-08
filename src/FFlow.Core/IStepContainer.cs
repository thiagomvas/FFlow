namespace FFlow.Core;

public interface IStepContainer
{
    FlowStep GetStep(string identifier);
    void AddStep<T>(string identifier) where T : FlowStep, new();
}