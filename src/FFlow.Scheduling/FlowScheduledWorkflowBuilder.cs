using FFlow.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FFlow.Scheduling;

internal class FflowScheduledWorkflowBuilder : IFflowScheduledWorkflowBuilder
{
    private readonly Type _definition;
    private readonly List<Action<IServiceProvider, IFlowScheduleStore>> _registrations;

    public FflowScheduledWorkflowBuilder(Type definition, List<Action<IServiceProvider, IFlowScheduleStore>> registrations)
    {
        _definition = definition;
        _registrations = registrations;
    }

    public void RunEvery(string cron)
    {
        _registrations.Add((provider, store) =>
            store.AddAsync(ScheduledWorkflow.CreateRecurring((IWorkflowDefinition)provider.GetRequiredService(_definition), cron)).Wait());
    }

    public void RunEvery(TimeSpan interval)
    {
        _registrations.Add((provider, store) =>
            store.AddAsync(ScheduledWorkflow.CreateRecurring((IWorkflowDefinition)provider.GetRequiredService(_definition), interval)).Wait());
    }

    public void RunOnceAt(DateTime timeUtc)
    {
        _registrations.Add((provider, store) =>
            store.AddAsync(ScheduledWorkflow.CreateOneTime((IWorkflowDefinition)provider.GetRequiredService(_definition), timeUtc)).Wait());
    }
}