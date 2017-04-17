using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace HydraDoc.CSS
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}