using FFlow.Core;

namespace FFlow.Demo;

public class DemoStepRegistry : IStepRegistry
{
    public void RegisterSteps(IStepContainer container)
    {
        container.AddStep<HelloStep>("demo.hello");
    }
}