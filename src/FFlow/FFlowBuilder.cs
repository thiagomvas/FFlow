using FFlow.Core;

namespace FFlow;

public class FFlowBuilder : IWorkflowBuilder, IConfigurableWorkflowBuilder
{
    private WorkflowOptions? _options { get; set;}
    private readonly List<IFlowStep> _steps = new List<IFlowStep>();
    private IFlowStep? _errorHandler;
    private IFlowStep? _finalizer;
    private IServiceProvider? _serviceProvider;
    private Type? _contextType = typeof(InMemoryFFLowContext);
    private IFlowContext? _contextInstance;
    private readonly IStepTemplateRegistry? _templateRegistry;
    
    /// <summary>
    /// A read-only list of steps in the workflow builder.
    /// </summary>
    public IReadOnlyList<IFlowStep> Steps => _steps.AsReadOnly();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FFlowBuilder"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider to resolve dependencies for steps and context.
    /// </param>
    /// <param name="templateRegistry">
    /// The registry for step templates, allowing for default configurations and overrides.
    /// </param>
    /// <remarks>
    /// When <paramref name="templateRegistry"/> is <c>null</c>, it will use <see cref="StepTemplateRegistry.Instance">the singleton instance</see> for <see cref="StepTemplateRegistry"/>.
    /// </remarks>
    public FFlowBuilder(IServiceProvider? serviceProvider, IStepTemplateRegistry? templateRegistry = null)
    {
        _serviceProvider = serviceProvider;
        _options = new();
        _templateRegistry = templateRegistry;
        if (_templateRegistry is null)
        {
            _templateRegistry = StepTemplateRegistry.Instance;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FFlowBuilder"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider to resolve dependencies for steps and context.
    /// </param>
    public FFlowBuilder(IServiceProvider? serviceProvider = null) : 
        this(serviceProvider, 
            (IStepTemplateRegistry?) serviceProvider?.GetService(typeof(IStepTemplateRegistry)))
    {
        
    }

    public IConfigurableStepBuilder AddStep(IFlowStep step)
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder StartWith<TStep>() where TStep : class, IFlowStep
    {
        var step = Internals.GetOrCreateStep<TStep>(_serviceProvider);
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder StartWith(AsyncFlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder StartWith(SyncFlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var asyncAction = new AsyncFlowAction((context, cancellationToken) =>
        {
            setupAction(context, cancellationToken);
            return Task.CompletedTask;
        });
        
        var step = new DelegateFlowStep(asyncAction);
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder Then<TStep>() where TStep : class, IFlowStep
    {
        var step = Internals.GetOrCreateStep<TStep>(_serviceProvider);
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder Then(AsyncFlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder Then(SyncFlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var asyncAction = new AsyncFlowAction((context, cancellationToken) =>
        {
            setupAction(context, cancellationToken);
            return Task.CompletedTask;
        });
        
        var step = new DelegateFlowStep(asyncAction);
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder Finally<TStep>() where TStep : class, IFlowStep
    {
        var step = Internals.GetOrCreateStep<TStep>(_serviceProvider);
        _finalizer = step;
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder Finally(AsyncFlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var step = new DelegateFlowStep(setupAction);
        _finalizer = step;
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder Finally(SyncFlowAction setupAction)
    {
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
        
        var asyncAction = new AsyncFlowAction((context, cancellationToken) =>
        {
            setupAction(context, cancellationToken);
            return Task.CompletedTask;
        });
        
        var step = new DelegateFlowStep(asyncAction);
        _finalizer = step;
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder If(Func<IFlowContext, bool> condition, AsyncFlowAction then, AsyncFlowAction? otherwise = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (then == null) throw new ArgumentNullException(nameof(then));
        
        var trueStep = new DelegateFlowStep(then);
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            falseStep = new DelegateFlowStep(otherwise);
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return CreateStepBuilder(ifStep);
    }

    public IConfigurableStepBuilder If<TTrue>(Func<IFlowContext, bool> condition, AsyncFlowAction? otherwise = null) where TTrue : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        
        var trueStep = Internals.GetOrCreateStep<TTrue>(_serviceProvider);
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            falseStep = new DelegateFlowStep(otherwise);
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return CreateStepBuilder(ifStep);
    }

    public IConfigurableStepBuilder If(Func<IFlowContext, bool> condition, SyncFlowAction then, SyncFlowAction? otherwise = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (then == null) throw new ArgumentNullException(nameof(then));
        
        var trueStep = new DelegateFlowStep((context, cancellationToken) =>
        {
            then(context, cancellationToken);
            return Task.CompletedTask;
        });
        
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            falseStep = new DelegateFlowStep((context, cancellationToken) =>
            {
                otherwise(context, cancellationToken);
                return Task.CompletedTask;
            });
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return CreateStepBuilder(ifStep);
    }

    public IConfigurableStepBuilder If<TTrue>(Func<IFlowContext, bool> condition, SyncFlowAction? otherwise = null) where TTrue : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        
        var trueStep = Internals.GetOrCreateStep<TTrue>(_serviceProvider);
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            falseStep = new DelegateFlowStep((context, cancellationToken) =>
            {
                otherwise(context, cancellationToken);
                return Task.CompletedTask;
            });
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return CreateStepBuilder(ifStep);
    }

    public IConfigurableStepBuilder If<TTrue, TFalse>(Func<IFlowContext, bool> condition) where TTrue : class, IFlowStep where TFalse : class, IFlowStep
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        
        var trueStep = Internals.GetOrCreateStep<TTrue>(_serviceProvider);
        var falseStep = Internals.GetOrCreateStep<TFalse>(_serviceProvider);

        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return CreateStepBuilder(ifStep);
    }

    public IConfigurableStepBuilder If(Func<IFlowContext, bool> condition, Func<IWorkflowBuilder> then, Func<IWorkflowBuilder>? otherwise = null)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (then == null) throw new ArgumentNullException(nameof(then));
        
        var trueBuilder = then();
        var trueStep = new BuilderStep(trueBuilder);
        IFlowStep? falseStep = null;
        
        if (otherwise != null)
        {
            var falseBuilder = otherwise();
            falseStep = new BuilderStep(falseBuilder);
        }
        
        var ifStep = new IfStep(condition, trueStep, falseStep);
        _steps.Add(ifStep);
        return CreateStepBuilder(ifStep);
    }

    public IConfigurableStepBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, AsyncFlowAction action)
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var step = new ForEachStep(itemsSelector, new DelegateFlowStep(action));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, AsyncFlowAction action) where TItem : class
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var step = new ForEachStep<TItem>(itemsSelector, new DelegateFlowStep(action));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, SyncFlowAction action)
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var asyncAction = new AsyncFlowAction((context, cancellationToken) =>
        {
            action(context, cancellationToken);
            return Task.CompletedTask;
        });
        
        var step = new ForEachStep(itemsSelector, new DelegateFlowStep(asyncAction));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, SyncFlowAction action) where TItem : class
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var asyncAction = new AsyncFlowAction((context, cancellationToken) =>
        {
            action(context, cancellationToken);
            return Task.CompletedTask;
        });
        
        var step = new ForEachStep<TItem>(itemsSelector, new DelegateFlowStep(asyncAction));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ForEach<TStepIterator>(Func<IFlowContext, IEnumerable<object>> itemsSelector) where TStepIterator : class, IFlowStep
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        
        var step = new ForEachStep(itemsSelector, Internals.GetOrCreateStep<TStepIterator>(_serviceProvider));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ForEach<TStepIterator, TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector) where TStepIterator : class, IFlowStep
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        
        var step = new ForEachStep<TItem>(itemsSelector, Internals.GetOrCreateStep<TStepIterator>(_serviceProvider));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ForEach(Func<IFlowContext, IEnumerable<object>> itemsSelector, Func<IWorkflowBuilder> action)
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var builder = action();
        var step = new ForEachStep(itemsSelector, new BuilderStep(builder));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ForEach<TItem>(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Func<IWorkflowBuilder> action)
    {
        if (itemsSelector == null) throw new ArgumentNullException(nameof(itemsSelector));
        if (action == null) throw new ArgumentNullException(nameof(action));
        
        var builder = action();
        var step = new ForEachStep<TItem>(itemsSelector, new BuilderStep(builder));
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder ContinueWith<TWorkflowDefinition>() where TWorkflowDefinition : class, IWorkflowDefinition
    {
        var workflowDefinition = _serviceProvider?.GetService(typeof(TWorkflowDefinition)) as TWorkflowDefinition
                                 ?? Activator.CreateInstance<TWorkflowDefinition>();
        if (workflowDefinition == null)
        {
            throw new InvalidOperationException($"Could not create instance of {typeof(TWorkflowDefinition).Name}");
        }
        
        
        
        var step = new WorkflowContinuationStep(workflowDefinition);
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IConfigurableStepBuilder Switch(Action<ISwitchCaseBuilder> caseBuilder)
    {
        if (caseBuilder == null) throw new ArgumentNullException(nameof(caseBuilder));
        
        var switchCaseBuilder = new SwitchCaseBuilder { _serviceProvider = _serviceProvider };
        caseBuilder(switchCaseBuilder);
        
        var step = switchCaseBuilder.Build();
        
        _steps.Add(step);
        return CreateStepBuilder(step);
    }

    public IWorkflowBuilder Fork(ForkStrategy strategy, params Func<IWorkflowBuilder>[] forks)
    {
        var step = new ForkStep(strategy, []);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder UseContext<TContext>() where TContext : class, IFlowContext
    {
        _contextType = typeof(TContext);
        return this;
    }

    public IWorkflowBuilder UseContext(IFlowContext context)
    {
        _contextInstance = context;
        return this;
    }

    public IWorkflowBuilder SetProvider(IServiceProvider provider)
    {
        _serviceProvider = provider;
        return this;
    }


    public IWorkflowBuilder OnAnyError<TStep>() where TStep : class, IFlowStep
    {
        var step = Internals.GetOrCreateStep<TStep>(_serviceProvider);
        _errorHandler = step ?? throw new InvalidOperationException($"Could not create instance of {typeof(TStep).Name}");
        return this;
    }

    public IWorkflowBuilder OnAnyError(AsyncFlowAction errorHandlerAction)
    {
        if (errorHandlerAction == null) throw new ArgumentNullException(nameof(errorHandlerAction));
        
        _errorHandler = new DelegateFlowStep(errorHandlerAction);
        return this;
    }

    public IWorkflowBuilder OnAnyError(SyncFlowAction errorHandlerAction)
    {
        if (errorHandlerAction == null) throw new ArgumentNullException(nameof(errorHandlerAction));
        
        var asyncAction = new AsyncFlowAction((context, cancellationToken) =>
        {
            errorHandlerAction(context, cancellationToken);
            return Task.CompletedTask;
        });
        
        _errorHandler = new DelegateFlowStep(asyncAction);
        return this;
    }

    public IWorkflowBuilder Delay(int milliseconds)
    {
        if (milliseconds < 0) throw new ArgumentOutOfRangeException(nameof(milliseconds), "Delay must be a non-negative value.");
        
        var step = new DelayStep(milliseconds);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder Delay(TimeSpan delay)
    {
        if (delay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(delay), "Delay must be a non-negative value.");
        
        var step = new DelayStep(delay);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder Throw(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        
        var step = new ThrowExceptionStep(message);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder Throw<TException>(string message) where TException : Exception, new()
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));

        var exception = (TException)Activator.CreateInstance(typeof(TException), message)!
                        ?? throw new InvalidOperationException($"Could not create instance of {typeof(TException)} with message.");

        var step = new ThrowExceptionStep(exception);
        _steps.Add(step);
        return this;
    }


    public IWorkflowBuilder ThrowIf(Func<IFlowContext, bool> condition, string message)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        
        var step = new ThrowExceptionIfStep(condition, message);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder ThrowIf<TException>(Func<IFlowContext, bool> condition, string message) where TException : Exception, new()
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        
        var exception = (TException)Activator.CreateInstance(typeof(TException), message)!
                        ?? throw new InvalidOperationException($"Could not create instance of {typeof(TException)} with message.");
        
        var step = new ThrowExceptionIfStep(condition, exception);
        _steps.Add(step);
        return this;
    }

    public IWorkflowBuilder InsertStepAt(int index, IFlowStep step)
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        if (index < 0 || index > _steps.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of existing steps.");
        
        _steps.Insert(index, step);
        return this;
    }

    public int GetStepCount()
    {
        return _steps.Count;
    }

    public IWorkflowBuilder WithDecorator<TDecorator>(Func<IFlowStep, TDecorator> decoratorFactory) where TDecorator : BaseStepDecorator
    {
        if (decoratorFactory == null) throw new ArgumentNullException(nameof(decoratorFactory));

        _options?.AddStepDecorator(decoratorFactory);
        
        return this;
    }

    public IWorkflow Build()
    {
        var context = _contextInstance
                      ?? _serviceProvider?.GetService(_contextType ?? typeof(InMemoryFFLowContext)) as IFlowContext
                      ?? Activator.CreateInstance(_contextType ?? typeof(InMemoryFFLowContext)) as IFlowContext;
        
        var result = new Workflow(_steps, context!, _options);
        
        if (_errorHandler != null)
        {
            result.SetGlobalErrorHandler(_errorHandler);
        }

        if (_finalizer is not null) result.SetFinalizer(_finalizer);
        
        return result;
    }
    public IWorkflowBuilder WithOptions(Action<WorkflowOptions> configure)
    {
        if (configure == null) throw new ArgumentNullException(nameof(configure));
        
        _options ??= new WorkflowOptions();
        configure(_options);
        return this;
    }
    
    private FlowStepBuilder CreateStepBuilder(IFlowStep step)
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        
        if (_templateRegistry.TryGetOverridenDefaults(step.GetType(), out var configure))
        {
            configure(step);
        }
        
        return new FlowStepBuilder(this, step, _templateRegistry);
    }
    
    private FlowStepBuilder<TStep> CreateStepBuilder<TStep>(TStep step) where TStep : class, IFlowStep
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        
        if (_templateRegistry.TryGetOverridenDefaults(typeof(TStep), out var configure))
        {
            configure(step);
        }
        
        return new FlowStepBuilder<TStep>(this, step, _templateRegistry);
    }
}