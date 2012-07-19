using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Archimedes.Patterns.RegEx
{
    public class RegExReplacer
    {
        private readonly Regex _regularExpression;
        private readonly String _replacement;

        protected RegExReplacer() { }

        public RegExReplacer(String regex, String replacement)
            : this(new Regex(regex), replacement) { }

        public RegExReplacer(Regex regex, String replacement) {
            _regularExpression = regex;
            _replacement = replacement;
        }

        public virtual String Replace(String input) {
            return _regularExpression.Replace(input, _replacement);
        }

    }
}
