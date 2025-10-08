using System.Text.Json;
using FFlow.Core;

namespace FFlow.DSL;

public class Interpreter
{
    private readonly StepContainer _container;

    public Interpreter()
    {
        _container = new StepContainer();
        _container.LoadAllRegistries();
        
    }
    
    public IWorkflow Interpret(PipelineNode pipeline)
    {
        var builder = new FFlowBuilder();
        foreach (var stepNode in pipeline.Steps)
        {
            var step = _container.GetStep(stepNode.Identifier, stepNode.Parameters);
            if (step == null)
            {
                throw new Exception($"Step '{stepNode.Identifier}' not found in container.");
            }

            builder.AddStep(step);
        }

        return builder.Build();
    }
    
}