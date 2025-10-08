namespace FFlow.DSL;

public class StepNode
{
    public string Identifier { get; }
    public Dictionary<string, object> Parameters { get; }
    
    public StepNode(string identifier, Dictionary<string, object> parameters)
    {
        Identifier = identifier;
        Parameters = parameters;
    }
}