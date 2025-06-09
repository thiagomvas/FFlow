using FFlow.Core;

namespace Workflow.Tests.Shared;

public class DiStep : IFlowStep
{
    private readonly TestService _service;
    
    public DiStep(TestService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_service == null) throw new InvalidOperationException("Service must be set.");
        
        cancellationToken.ThrowIfCancellationRequested();
        
        // Simulate some work with the service
        _service.DoSomething();
        
        return Task.CompletedTask;
    }
}