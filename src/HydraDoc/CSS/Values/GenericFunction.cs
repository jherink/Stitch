using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace HydraDoc.CSS
{
    public class GenericFunction : Term
    {
        public string Name { get; set; }
        public TermList Arguments { get; set; }

        public GenericFunction(string name, IEnumerable<Term> arguments)
        {
            Name = name;
            var list = new TermList();
            foreach (var term in arguments)
            {
                list.AddTerm(term);
            }
            Arguments = list;
        }

        public override string ToString()
        {
            return Name + "(" + Arguments + ")";
        }
    }
}