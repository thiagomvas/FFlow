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
    
    public static void SetDotnetBuildConfig(this IFlowContext context, DotnetBuildConfiguration config) 
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetBuildConfigKey, config);
    }
    
    public static DotnetBuildConfiguration GetDotnetBuildConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetBuildConfiguration>(Internals.DotnetBuildConfigKey);
    }
    
    public static void SetDotnetRestoreConfig(this IFlowContext context, DotnetRestoreConfiguration config) 
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetRestoreConfigKey, config);
    }
    
    public static DotnetRestoreConfiguration GetDotnetRestoreConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetRestoreConfiguration>(Internals.DotnetRestoreConfigKey);
    }
    
    public static void SetDotnetTestConfig(this IFlowContext context, DotnetTestConfiguration config) 
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetTestConfigKey, config);
    }
    
    public static DotnetTestConfiguration GetDotnetTestConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetTestConfiguration>(Internals.DotnetTestConfigKey);
    }
    
    
}