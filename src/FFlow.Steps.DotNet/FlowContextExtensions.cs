using FFlow.Core;

namespace FFlow.Steps.DotNet;

public static class FlowContextExtensions
{
    
    public static void SetTargetSolution(this IFlowContext context, string solutionPath)
    {
        context.Set($"{Internals.BaseNamespace}.{Internals.TargetSolutionKey}", solutionPath);
    }
    
    public static string GetTargetSolution(this IFlowContext context)
    {
        return context.Get<string>($"{Internals.BaseNamespace}.{Internals.TargetSolutionKey}");
    }
    
    public static void SetTargetProject(this IFlowContext context, string projectPath)
    {
        context.Set($"{Internals.BaseNamespace}.{Internals.TargetProjectKey}", projectPath);
    }
    
    public static string GetTargetProject(this IFlowContext context)
    {
        return context.Get<string>($"{Internals.BaseNamespace}.{Internals.TargetProjectKey}");
    }
    
    
}