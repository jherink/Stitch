using System.Collections.Generic;
using Stitch.CSS;
using Stitch.CSS.Extensions;

// ReSharper disable once CheckNamespace
namespace Stitch.CSS
{
    public class KeyframeRule : RuleSet, ISupportsDeclarations
    {
        private List<string> _values { get; set; }
        public KeyframeRule()
        {
            Declarations = new StyleDeclaration();
            RuleType = RuleType.Keyframe;
            _values = new List<string>();
        }

        public void AddValue(string value)
        {
            _values.Add(value);
        }

        public StyleDeclaration Declarations { get; private set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.Empty.Indent(friendlyFormat, indentation) +
                string.Join(",", _values) + 
                "{" + 
                Declarations.ToString(friendlyFormat, indentation) +
                "}".NewLineIndent(friendlyFormat, indentation);
        }
    }
}
