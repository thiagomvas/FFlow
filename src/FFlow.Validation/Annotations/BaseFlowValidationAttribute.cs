using FFlow.Core;

namespace FFlow.Validation.Annotations;

public abstract class BaseFlowValidationAttribute : Attribute
{
    public abstract IFlowStep CreateValidationStep();
}