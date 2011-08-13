using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Effects;
using System.Windows.Media;

namespace Archimedes.Patterns.WPF
{
    public class GlobalEffects
    {
        static GlobalEffects() {
            int blurRadius = 10;

            RedGlowEffect = new DropShadowEffect();
            RedGlowEffect.ShadowDepth = 0;
            RedGlowEffect.BlurRadius = blurRadius;
            RedGlowEffect.Color = Colors.Red;

            GoldGlowEffect = new DropShadowEffect();
            GoldGlowEffect.ShadowDepth = 0;
            GoldGlowEffect.BlurRadius = blurRadius;
            GoldGlowEffect.Color = Colors.Gold;

            BlueGlowEffect = new DropShadowEffect();
            BlueGlowEffect.ShadowDepth = 0;
            BlueGlowEffect.BlurRadius = blurRadius;
            BlueGlowEffect.Color = Colors.BlueViolet;
        }

        public static readonly DropShadowEffect RedGlowEffect;
        public static readonly DropShadowEffect GoldGlowEffect;
        public static readonly DropShadowEffect BlueGlowEffect;
    }
}
