using FFlow.Core;

namespace FFlow;

public class Workflow : IWorkflow
{
    public readonly Guid Id = Guid.NewGuid();
    private readonly IReadOnlyList<IFlowStep> _steps;
    private IFlowContext _context;
    private IFlowStep _globalErrorHandler;
    
    public Workflow(IReadOnlyList<IFlowStep> steps, IFlowContext context)
    {
        _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public IWorkflow SetGlobalErrorHandler(IFlowStep errorHandler)
    {
        _globalErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        return this;
    }

    public IWorkflow SetContext(IFlowContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        return this;
    }

    public async Task<IFlowContext> RunAsync(object input, CancellationToken cancellationToken = default)
    {
        _context.SetId(Id);
        if(input is not null)
            _context.SetInput(input);
        try 
        {
            ParallelStepTracker.Instance.Initialize(Id);
            
            foreach (var step in _steps)
            {
                await step.RunAsync(_context, cancellationToken);
            }

            await ParallelStepTracker.Instance.WaitForAllTasksAsync(Id);
        }
        catch (Exception ex)
        {
            if (_globalErrorHandler != null)
            {
                _context.Set("Exception", ex);
                await _globalErrorHandler.RunAsync(_context);
            }
            else
            {
                throw;
            }
        }

        return _context;
    }
}