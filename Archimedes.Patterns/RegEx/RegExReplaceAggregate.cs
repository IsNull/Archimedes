using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.RegEx
{
    public class RegExReplaceAggregate : RegExReplacer
    {
        private readonly List<RegExReplacer> _replacers = new List<RegExReplacer>();

        public RegExReplaceAggregate() { }

        public RegExReplaceAggregate(IEnumerable<RegExReplacer> replacers) {
            this.Add(replacers);
        }

        public void Add(IEnumerable<RegExReplacer> replacers) {
            _replacers.AddRange(replacers);
        }

        public void Add(RegExReplacer replacer) {
            _replacers.Add(replacer);
        }

        public override string Replace(string input) {
            foreach (var rep in _replacers)
                input = rep.Replace(input);
            return input;
        }

    }
}
