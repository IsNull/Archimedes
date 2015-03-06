using System;
using Archimedes.Geometry.Primitives;
using NUnit.Framework;

namespace Archimedes.Geometry.Tests
{
    public class Line2Tests
    {
        
        [TestCase("10, 10", "100, 100")]
        public void Constructor(string sp, string ep)
        {
            var spv = Vector2.Parse(sp);
            var epv = Vector2.Parse(ep);

            var l1 = new Line2(spv, epv);

            Assert.AreEqual(l1.Start, spv);
            Assert.AreEqual(l1.End, epv);
        }

        [TestCase("(10,10) (100,10)", 90)]  // Horizontal
        public void Parse(string sp, double expected)
        {
            var l1 = Line2.Parse(sp);
            Assert.AreEqual(expected, l1.Lenght);
        }


        [TestCase("10, 10", "100, 10", 90)]
        [TestCase("10, 10", "10, 10", 0)]
        [TestCase("10, 10", "10, 100", 90)]
        public void Lenght(string sp, string ep, double expected)
        {
            var spv = Vector2.Parse(sp);
            var epv = Vector2.Parse(ep);

            var l1 = new Line2(spv, epv);

            Assert.AreEqual(l1.Lenght, expected);
        }

        #region Slopes / Horz & Vert

        [TestCase("10, 10", "100, 10", 0)]  // Horizontal
        [TestCase("10, 10", "10, 10", 0)]   // Only a point
        [TestCase("10, 10", "10, 100", 0)]  // Vertical
        [TestCase("0, 0", "100, 100", 1)]
        [TestCase("0, 0", "100, 200", 2)]
        public void Slope(string sp, string ep, double expected)
        {
            var spv = Vector2.Parse(sp);
            var epv = Vector2.Parse(ep);
            var l1 = new Line2(spv, epv);

            Assert.AreEqual(expected, l1.Slope);
        }

        [TestCase("10, 10", "100, 10", false)]  // Horizontal
        [TestCase("10, 10", "10, 10", true)]   // Only a point
        [TestCase("10, 10", "10, 100", true)]  // Vertical
        [TestCase("0, 0", "100, 100", false)]
        [TestCase("0, 0", "100, 200", false)]
        public void IsVertical(string sp, string ep, bool expected)
        {
            var spv = Vector2.Parse(sp);
            var epv = Vector2.Parse(ep);
            var l1 = new Line2(spv, epv);

            Assert.AreEqual(expected, l1.IsVertical);
        }

        [TestCase("10, 10", "100, 10", true)]  // Horizontal
        [TestCase("10, 10", "10, 10", true)]   // Only a point
        [TestCase("10, 10", "10, 100", false)]  // Vertical
        [TestCase("0, 0", "100, 100", false)]
        [TestCase("0, 0", "100, 200", false)]
        public void IsHorizontal(string sp, string ep, bool expected)
        {
            var spv = Vector2.Parse(sp);
            var epv = Vector2.Parse(ep);
            var l1 = new Line2(spv, epv);

            Assert.AreEqual(expected, l1.IsHorizontal);
        }

        [TestCase("(10,10) (100,10)", "(10,10) (100,10)", true)] 
        [TestCase("(10,10) (100,10)","(10,10) (10,100)", false)]
        [TestCase("(10,10) (100,10)", "(0,500) (10,500)", true)] // Two horz lines
        [TestCase("(10,10) (10,100)", "(0,303) (0,500)", true)] // Two vert lines
        public void IsParallelTo(string line1Str, string line2Str, bool expected)
        {
            var line1 = Line2.Parse(line1Str);
            var line2 = Line2.Parse(line2Str);
            Assert.AreEqual(expected, line1.IsParallelTo(line2));
        }
         

        #endregion

    }
}
