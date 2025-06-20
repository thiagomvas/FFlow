using FFlow.Core;

namespace FFlow;

public class Workflow : IWorkflow
{
    public readonly Guid Id = Guid.NewGuid();
    private readonly IReadOnlyList<IFlowStep> _steps;
    private IFlowContext _context;
    private IFlowStep? _globalErrorHandler;
    private IFlowStep? _finalizer;
    private readonly WorkflowOptions? _options;

    public Workflow(IReadOnlyList<IFlowStep> steps, IFlowContext context, WorkflowOptions? options = null)
    {
        _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _options = options;
        if (options?.StepDecoratorFactory is not null)
        {
            var decoratedSteps = new List<IFlowStep>();
            foreach (var step in _steps)
            {
                decoratedSteps.Add(options.StepDecoratorFactory(step));
            }

            _steps = decoratedSteps.AsReadOnly();
        }
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

    public IWorkflow SetFinalizer(IFlowStep finalizer)
    {
        _finalizer = finalizer ?? throw new ArgumentNullException(nameof(finalizer));
        return this;
    }

    public async Task<IFlowContext> RunAsync(object input, CancellationToken cancellationToken = default)
    {
        _context.SetId(Id);
        if (input is not null)
            _context.SetInput(input);


        var eventListener = _options?.EventListener;
        eventListener?.OnWorkflowStarted(this);
        IFlowStep? current = null;
        try
        {
            ParallelStepTracker.Instance.Initialize(Id);

            using var globalCts = _options?.GlobalTimeout is { } timeout
                ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
                : null;

            if (_options?.GlobalTimeout is not null)
                globalCts.CancelAfter(_options.GlobalTimeout.Value);

            var effectiveToken = globalCts?.Token ?? cancellationToken;

            foreach (var step in _steps)
            {
                eventListener?.OnStepStarted(step, _context);
                current = step;
                if (_options?.StepTimeout is not null)
                {
                    using var stepCts = CancellationTokenSource.CreateLinkedTokenSource(effectiveToken);
                    stepCts.CancelAfter(_options.StepTimeout.Value);
                    await step.RunAsync(_context, stepCts.Token);
                }
                else
                {
                    await step.RunAsync(_context, effectiveToken);
                }

                eventListener?.OnStepCompleted(step, _context);
            }

            if (_options?.StepTimeout is not null)
            {
                using var stepCts = CancellationTokenSource.CreateLinkedTokenSource(effectiveToken);
                stepCts.CancelAfter(_options.StepTimeout.Value);
                await ParallelStepTracker.Instance.WaitForAllTasksAsync(Id, stepCts.Token);
            }
            else
            {
                await ParallelStepTracker.Instance.WaitForAllTasksAsync(Id, effectiveToken);
            }
        }
        catch (Exception ex)
        {
            if (_finalizer != null)
            {
                await _finalizer.RunAsync(_context, cancellationToken);
            }

            if (current is not null)
                eventListener?.OnStepFailed(current, _context, ex);

            eventListener?.OnWorkflowFailed(this, ex);
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

        if (_finalizer != null)
        {
            await _finalizer.RunAsync(_context, cancellationToken);
        }

        eventListener?.OnWorkflowCompleted(this);

        return _context;
    }
}