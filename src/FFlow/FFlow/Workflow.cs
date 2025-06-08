using FFlow.Core;

namespace FFlow;

public class Workflow<TInput> : IWorkflow<TInput>
{
    private readonly IReadOnlyList<IFlowStep> _steps;
    private readonly IFlowContext _context;
    
    public Workflow(IReadOnlyList<IFlowStep> steps, IFlowContext context)
    {
        _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task RunAsync(TInput input)
    {
        foreach (var step in _steps)
        {
            await step.RunAsync(_context);
        }
    }
}