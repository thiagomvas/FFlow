using System.Collections;
using FFlow.Core;

namespace FFlow;

internal class ForEachStep<TItem, TStepIterator> : FlowStep
    where TStepIterator : IFlowStep
{
    private readonly Func<IFlowContext, IEnumerable<TItem>> _itemsSelector;
    private readonly Action<TItem>? _executor;
    private readonly Action<TItem, TStepIterator>? _configurator;
    private readonly Func<TStepIterator> _stepFactory;

    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem> executor)
    {
        _itemsSelector = itemsSelector ?? throw new ArgumentNullException(nameof(itemsSelector));
        _executor = executor ?? throw new ArgumentNullException(nameof(executor));
    }

    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Func<TStepIterator> factory,
        Action<TItem, TStepIterator> configurator)
    {
        _itemsSelector = itemsSelector ?? throw new ArgumentNullException(nameof(itemsSelector));
        _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));
        _stepFactory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        var items = _itemsSelector(context);
        if (items is null || !items.Any()) return;

        foreach (var item in items)
        {
            if (_executor != null)
            {
                _executor(item);
            }
            else if (_configurator != null)
            {
                var step = _stepFactory();
                _configurator(item, step);
                await step.RunAsync(context);
            }
            else
            {
                throw new InvalidOperationException("Either executor or configurator must be provided.");
            }
        }
    }
}

internal class ForEachStep<TItem> : ForEachStep<TItem, FlowStep>
{
    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem> executor)
        : base(itemsSelector, executor)
    {
    }

    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem, FlowStep> configurator)
        : base(itemsSelector, null, configurator)
    {
    }
}


internal class ForEachStep : ForEachStep<object, FlowStep>
{
    public ForEachStep(Func<IFlowContext, IEnumerable<object>> itemsSelector, Action<object> executor)
        : base(context => itemsSelector(context), executor)
    {
    }

    public ForEachStep(Func<IFlowContext, IEnumerable<object>> itemsSelector, Action<object, FlowStep> configurator)
        : base(context => itemsSelector(context), null, configurator)
    {
    }
}