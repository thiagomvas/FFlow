using FFlow.Core;
using FFlow.Exceptions;

namespace FFlow;

internal class Internals
{
    internal const string FFlowContextInputKey = "fflow.ctx.input";

    internal static TStep GetOrCreateStep<TStep>(IServiceProvider? serviceProvider) where TStep : class, IFlowStep
    {
        if (serviceProvider != null)
        {
            var step = (TStep) serviceProvider.GetService(typeof(TStep));
            if (step != null)
                return step;
        }

        // Fallback only if TStep has a public parameterless constructor
        var constructor = typeof(TStep).GetConstructor(Type.EmptyTypes);
        if (constructor != null)
        {
            return Activator.CreateInstance<TStep>();
        }

        throw new StepCreationException(typeof(TStep));
    }

    
}