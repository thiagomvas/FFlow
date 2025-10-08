namespace FFlow.DSL;

public class PipelineNode
{
    public string Name { get; }
    public List<StepNode> Steps { get; }
    
    public PipelineNode(string name, List<StepNode> steps)
    {
        Name = name;
        Steps = steps;
    }
}