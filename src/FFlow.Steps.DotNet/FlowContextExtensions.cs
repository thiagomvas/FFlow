using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// Provides extension methods for managing .NET build, restore, test, pack, run, and publish configurations
/// within the workflow context.
/// </summary>
public static class FlowContextExtensions
{
    /// <summary>
    /// Sets the .NET build configuration in the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <param name="config">The .NET build configuration to set.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="config"/> is null.</exception>
    public static void SetDotnetBuildConfig(this IFlowContext context, DotnetBuildConfiguration config)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetBuildConfigKey, config);
    }

    /// <summary>
    /// Retrieves the .NET build configuration from the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <returns>The .NET build configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    public static DotnetBuildConfiguration GetDotnetBuildConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetBuildConfiguration>(Internals.DotnetBuildConfigKey);
    }

    /// <summary>
    /// Sets the .NET restore configuration in the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <param name="config">The .NET restore configuration to set.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="config"/> is null.</exception>
    public static void SetDotnetRestoreConfig(this IFlowContext context, DotnetRestoreConfiguration config)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetRestoreConfigKey, config);
    }

    /// <summary>
    /// Retrieves the .NET restore configuration from the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <returns>The .NET restore configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    public static DotnetRestoreConfiguration GetDotnetRestoreConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetRestoreConfiguration>(Internals.DotnetRestoreConfigKey);
    }

    /// <summary>
    /// Sets the .NET test configuration in the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <param name="config">The .NET test configuration to set.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="config"/> is null.</exception>
    public static void SetDotnetTestConfig(this IFlowContext context, DotnetTestConfiguration config)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetTestConfigKey, config);
    }

    /// <summary>
    /// Retrieves the .NET test configuration from the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <returns>The .NET test configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    public static DotnetTestConfiguration GetDotnetTestConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetTestConfiguration>(Internals.DotnetTestConfigKey);
    }

    /// <summary>
    /// Sets the .NET pack configuration in the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <param name="config">The .NET pack configuration to set.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="config"/> is null.</exception>
    public static void SetDotnetPackConfig(this IFlowContext context, DotnetPackConfiguration config)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetPackConfigKey, config);
    }

    /// <summary>
    /// Retrieves the .NET pack configuration from the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <returns>The .NET pack configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    public static DotnetPackConfiguration GetDotnetPackConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetPackConfiguration>(Internals.DotnetPackConfigKey);
    }

    /// <summary>
    /// Sets the .NET run configuration in the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <param name="config">The .NET run configuration to set.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="config"/> is null.</exception>
    public static void SetDotnetRunConfig(this IFlowContext context, DotnetRunConfiguration config)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetRunConfigKey, config);
    }

    /// <summary>
    /// Retrieves the .NET run configuration from the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <returns>The .NET run configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    public static DotnetRunConfiguration GetDotnetRunConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetRunConfiguration>(Internals.DotnetRunConfigKey);
    }

    /// <summary>
    /// Sets the .NET publish configuration in the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <param name="config">The .NET publish configuration to set.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="config"/> is null.</exception>
    public static void SetDotnetPublishConfig(this IFlowContext context, DotnetPublishConfiguration config)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (config == null) throw new ArgumentNullException(nameof(config));

        context.Set(Internals.DotnetPublishConfigKey, config);
    }

    /// <summary>
    /// Retrieves the .NET publish configuration from the workflow context.
    /// </summary>
    /// <param name="context">The workflow context.</param>
    /// <returns>The .NET publish configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    public static DotnetPublishConfiguration GetDotnetPublishConfig(this IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Get<DotnetPublishConfiguration>(Internals.DotnetPublishConfigKey);
    }
}