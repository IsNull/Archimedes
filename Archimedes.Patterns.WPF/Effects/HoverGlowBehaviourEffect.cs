using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Effects;
using System.Windows;
using Archimedes.Patterns.WPF.Behaviours;
using System.Windows.Input;

namespace Archimedes.Patterns.WPF.Effects
{
    public class HoverGlowBehaviourEffect : ControlBehaviour
    {
        Effect hovereffect;
        Effect backup = null;

        #region Constructor

        public HoverGlowBehaviourEffect(FrameworkElement e, Effect effect)
            : base(e) {
            hovereffect = effect;
        }

        public HoverGlowBehaviourEffect(FrameworkElement e)
            : this(e, GlobalEffects.RedGlowEffect) { }

        #endregion

        #region  Mouse Hover

        private void OnMouseEnter(object sender, MouseEventArgs e) {
            backup = AttachedElement.Effect;
            AttachedElement.Effect = hovereffect;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e) {
            AttachedElement.Effect = backup;
        }

        #endregion

        protected override void Attach(FrameworkElement e) {
            e.MouseEnter += OnMouseEnter;
            e.MouseLeave += OnMouseLeave;
        }
    }
}
