using FFlow.Core;

namespace FFlow.Steps.DotNet;

public static class FlowContextExtensions
{
    public static void SetDotnetConfiguration(this IFlowContext context, DotnetFlowConfiguration configuration)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        context.Set($"{Internals.BaseNamespace}.Configuration", configuration);
        
    }
    
    
}