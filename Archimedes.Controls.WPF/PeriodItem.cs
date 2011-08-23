using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Types;

namespace Archimedes.Controls.WPF
{
    public class PeriodItem
    {
        public string Display { get; set; }
        public Period Time { get; set; }

        public PeriodItem(PeriodFormat format, Period time) {
            Display = time.ToString(format);
            Time = time;
        }

        #region Base Class Overrides

        public override bool Equals(object obj) {
            var item = obj as PeriodItem;
            if (item != null)
                return Time == item.Time;
            else
                return false;
        }

        public override int GetHashCode() {
            return Time.GetHashCode();
        }

        #endregion //Base Class Overrides
    }
}
