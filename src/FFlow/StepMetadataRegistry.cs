using FFlow.Core;

namespace FFlow;

public class StepMetadataRegistry
{
    private readonly Dictionary<Type, StepMetadata> _metadata = new();
    public static readonly Lazy<StepMetadataRegistry> Instance = new(() => new StepMetadataRegistry());
    public void Register(Type stepType)
    {
        if (stepType is null)
            throw new ArgumentNullException(nameof(stepType), "Step type cannot be null.");
        
        if (!typeof(IFlowStep).IsAssignableFrom(stepType))
            throw new ArgumentException($"Type '{stepType.FullName}' does not implement IFlowStep.", nameof(stepType));
        
        if (_metadata.ContainsKey(stepType))
            return;

        var attributes = stepType.GetCustomAttributes(inherit: true);
        
        var nameAttribute = attributes.OfType<StepNameAttribute>().FirstOrDefault();
        var tagsAttributes = attributes.OfType<StepTagsAttribute>();
        
        var metadata = new StepMetadata(
            Id: stepType.FullName ?? stepType.Name,
            Name: nameAttribute?.Name ?? stepType.Name,
            Tags: tagsAttributes.SelectMany(attr => attr.Tags).Distinct()
        );

        _metadata[stepType] = metadata;
    }
    
    public void Register<TStep>() where TStep : IFlowStep => Register(typeof(TStep));
    
    public StepMetadata GetMetadata(Type stepType)
    {
        if (stepType is null)
            throw new ArgumentNullException(nameof(stepType), "Step type cannot be null.");

        if (!typeof(IFlowStep).IsAssignableFrom(stepType))
            throw new ArgumentException($"Type '{stepType.FullName}' does not implement IFlowStep.", nameof(stepType));

        if (stepType.IsGenericType)
            stepType = stepType.GetGenericTypeDefinition();

        Register(stepType);
        return _metadata[stepType];
    }

    public StepMetadata GetMetadata<TStep>() where TStep : IFlowStep => GetMetadata(typeof(TStep));

    public IEnumerable<StepMetadata> GetAllMetadata() => _metadata.Values;
    
}