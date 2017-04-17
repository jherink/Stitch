using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace HydraDoc.CSS
{
    public abstract class AggregateRule : RuleSet, ISupportsRuleSets
    {
        protected AggregateRule()
        {
            RuleSets = new List<RuleSet>();
        }

        public List<RuleSet> RuleSets { get; private set; }
    }
}
