using System;
using NUnit.Framework;

namespace Archimedes.Geometry.Tests
{
    public class VectorTests
    {
        [TestCase("1, 0", "1, 0", 1e-4, true)]
        [TestCase("-1, 1", "-1, 1", 1e-4, true)]
        [TestCase("1, 0", "1, 1", 1e-4, false)]
        public void Equals(string v1s, string v2s, double tol, bool expected)
        {
            var v1 = Vector2.Parse(v1s);
            var v2 = Vector2.Parse(v2s);
            Assert.AreEqual(expected, v1 == v2);
            Assert.AreEqual(expected, v1.Equals(v2));
            Assert.AreEqual(expected, v1.Equals((object) v2));
            Assert.AreEqual(expected, Equals(v1, v2));
            Assert.AreEqual(expected, v1.Equals(v2, tol));
            Assert.AreNotEqual(expected, v1 != v2);
        }

        [TestCase("-1, -2", "1, 2", "0, 0")]
        public void Add(string v1s, string v2s, string evs)
        {
            var v1 = Vector2.Parse(v1s);
            var v2 = Vector2.Parse(v2s);
            var actuals = new[]
            {
                v1 + v2,
                v2 + v1,
            };
            var expected = Vector2.Parse(evs);
            foreach (var actual in actuals)
            {
                Assert.AreEqual(expected, actual);
            }
        }

        [TestCase("-1, -2", "1, 2", "-2, -4")]
        public void Subtract(string v1s, string v2s, string evs)
        {
            var v1 = Vector2.Parse(v1s);
            var v2 = Vector2.Parse(v2s);
            var actuals = new[]
            {
                v1 - v2,
            };
            var expected = Vector2.Parse(evs);
            foreach (var actual in actuals)
            {
                Assert.AreEqual(expected, actual);
            }
        }


        [TestCase("-1, -2", 2, "-2, -4")]
        public void MultiplyAndScaleBy(string vs, double d, string evs)
        {
            var v = Vector2.Parse(vs);
            var actuals = new[]
                          {
                              d * v,

                          };
            var expected = Vector2.Parse(evs);
            foreach (var actual in actuals)
            {
                Assert.AreEqual(expected, actual);
            }
        }

        [TestCase("-1, -2", 2, "-0.5, -1")]
        public void Divide(string vs, double d, string evs)
        {
            var v = Vector2.Parse(vs);
            var actual = v / d;
            var expected = Vector2.Parse(evs);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("2, 0", 2)]
        [TestCase("-2, 0", 2)]
        [TestCase("0, 2", 2)]
        public void Length(string vs, double expected)
        {
            var v = Vector2.Parse(vs);
            Assert.AreEqual(expected, v.Length, 1e-6);
        }
    }
}
