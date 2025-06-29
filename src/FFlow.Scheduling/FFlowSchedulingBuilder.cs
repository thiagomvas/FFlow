using FFlow.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FFlow.Scheduling;

internal class FflowSchedulingBuilder : IFflowSchedulingBuilder
{
    private readonly IServiceCollection _services;
    private readonly List<Action<IServiceProvider, IFlowScheduleStore>> _registrations = new();

    public FflowSchedulingBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public IFflowScheduledWorkflowBuilder AddWorkflow<TWorkflow>() where TWorkflow : IWorkflowDefinition
    {
        var builder = new FflowScheduledWorkflowBuilder(typeof(TWorkflow), _registrations);
        return builder;
    }

    public void ApplyRegistrations(IServiceProvider provider)
    {
        var store = provider.GetRequiredService<IFlowScheduleStore>();
        foreach (var registration in _registrations)
            registration(provider, store);
    }
}