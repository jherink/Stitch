using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Stitch.CSS
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}