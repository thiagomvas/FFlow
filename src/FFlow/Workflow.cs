using FFlow.Core;
using FFlow.Extensions;
using FFlow.Visualization;

namespace FFlow;

/// <summary>
/// An implementation of a workflow that manages a sequence of steps with backtracking capabilities.
/// </summary>
public class Workflow : IWorkflow
{
    public readonly Guid Id = Guid.CreateVersion7();

    private readonly ReversibleQueue<IFlowStep> _steps;
    private IFlowContext _context;
    private IFlowStep? _globalErrorHandler;
    private IFlowStep? _finalizer;
    private readonly WorkflowOptions? _options;
    private readonly IWorkflowMetadataStore? _metadataStore;
    public WorkflowGraph Graph { get; internal set; }
    private bool _isStopped;
    public IWorkflowMetadataStore? MetadataStore { get; internal set; }

    public Workflow(IReadOnlyList<IFlowStep> steps, IFlowContext context, WorkflowOptions? options = null,
        IWorkflowMetadataStore? metadataStore = null)
    {
        var result = steps.ToList();
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _options = options;
        MetadataStore = metadataStore ?? new InMemoryMetadataStore();
        if (options?.StepDecoratorFactory is not null)
        {
            var decoratedSteps = new List<IFlowStep>();
            foreach (var step in result)
            {
                decoratedSteps.Add(options.StepDecoratorFactory(step));
            }

            result = decoratedSteps;
        }
        
        _steps = new ReversibleQueue<IFlowStep>(result);
    }

    public IWorkflow SetGlobalErrorHandler(IFlowStep errorHandler)
    {
        _globalErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        return this;
    }

    public IFlowContext GetContext()
    {
        return _context;
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

    public async Task<IFlowContext> RunAsync(object? input = null, CancellationToken cancellationToken = default)
    {
        _context.SetId(Id);
        _context.SetSingleValue<IWorkflow>(this);
        _context.SetSingleValue<Workflow>(this);
        if (input is not null)
            _context.SetValue("Workflow.Input", input);

        if (_options.EventListener is CompositeFlowEventListener composite)
        {
            foreach (var listener in composite.Listeners)
            {
                _context.SetValue(Internals.BuildEventListenerKey(listener), listener);
            }
        }
        else if (_options?.EventListener is not null)
        {
            _context.SetValue(Internals.BuildEventListenerKey(_options.EventListener), _options.EventListener);
        }

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

            while(_steps.TryDequeue(out IFlowStep step))
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
                
                if (_isStopped)
                {
                    break;
                }
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
            while (_steps.TryBacktrack(out IFlowStep step))
            {
                if(step is ICompensableStep compensable)
                    await compensable.CompensateAsync(_context, cancellationToken);
            }
            
            if (_finalizer != null)
            {
                await _finalizer.RunAsync(_context, cancellationToken);
            }

            if (current is not null)
                eventListener?.OnStepFailed(current, _context, ex);

            eventListener?.OnWorkflowFailed(this, ex);

            if (_globalErrorHandler != null)
            {
                _context.SetSingleValue(ex);
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

    public async Task<IFlowContext> CompensateAsync(CancellationToken cancellationToken = default)
    {
        while (_steps.TryBacktrack(out IFlowStep step))
        {
            if (step is ICompensableStep compensable)
            {
                try
                {
                    await compensable.CompensateAsync(_context, cancellationToken);
                }
                catch (Exception ex)
                {
                    _context.SetSingleValue(ex);
                }
            }
        }
        return _context;
    }

    public void StopGracefully() => _isStopped = true;
}