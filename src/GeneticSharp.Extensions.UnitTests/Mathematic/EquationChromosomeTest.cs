using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Mathematic
{
    [TestFixture()]
    [Category("Extensions")]
    public class EqualtionChromosomeTest
    {
        [Test()]
        public void Constructor_ExpectedResult_Exception()
        {
            var actual = Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                new EquationChromosome(int.MaxValue, 2);
            }, "EquationChromosome expected value must be lower");

            ClassicAssert.AreEqual("expectedResult", actual.ParamName);
            ClassicAssert.AreEqual(actual.ActualValue, int.MaxValue);
        }

        [Test()]
        public void CreateNew_ExpectedResultAndLenth_Created()
        {
            var target = new EquationChromosome(10, 2);
            var newCreated = target.CreateNew() as EquationChromosome;
            ClassicAssert.AreEqual(target.Length, newCreated.Length);
            ClassicAssert.AreEqual(target.ResultRange, newCreated.ResultRange);
        }
    }
}