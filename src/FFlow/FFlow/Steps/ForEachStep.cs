using System.Collections;
using FFlow.Core;

namespace FFlow;

public class ForEachStep : ForEachStep<object>
{
    public ForEachStep(Func<IFlowContext, IEnumerable> itemsSelector, IFlowStep itemAction)
        : base(context => itemsSelector(context).Cast<object>(), itemAction)
    {
    }
}

public class ForEachStep<T> : IFlowStep
{
    private readonly Func<IFlowContext, IEnumerable<T>> _itemsSelector;
    private readonly IFlowStep _itemAction;
    
    public ForEachStep(Func<IFlowContext, IEnumerable<T>> itemsSelector, IFlowStep itemAction)
    {
        _itemsSelector = itemsSelector ?? throw new ArgumentNullException(nameof(itemsSelector));
        _itemAction = itemAction ?? throw new ArgumentNullException(nameof(itemAction));
    }

    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_itemsSelector == null) throw new InvalidOperationException("Items selector must be set.");
        if (_itemAction == null) throw new InvalidOperationException("Item action must be set.");

        var items = _itemsSelector(context);
        if (items == null) return;

        foreach (var item in items)
        {
            context.SetInput(item);
            await _itemAction.RunAsync(context, cancellationToken);
        }
    }
}