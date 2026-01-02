using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Tsp
{
    [TestFixture()]
    [Category("Extensions")]
    public class TspFitnessTest
    {
        [Test()]
        public void Constructor_MaxEqualsIntEdges_Exception()
        {
            var actual = Assert.Catch<ArgumentOutOfRangeException>(() => new TspFitness(10, 0, int.MaxValue, 0, 10000000));
            ClassicAssert.AreEqual("maxX", actual.ParamName);

            actual = Assert.Catch<ArgumentOutOfRangeException>(() => new TspFitness(10, 0, 10000000, 0, int.MaxValue));
            ClassicAssert.AreEqual("maxY", actual.ParamName);
        }

        [Test()]
        public void Evaluate_ChromosomeWithLowerCities_FitnessDividedByDiff()
        {
            var target = new TspFitness(10, 0, 10, 0, 10);
            var chromosome = new TspChromosome(9);

            var actual = target.Evaluate(chromosome);
            ClassicAssert.AreNotEqual(0, actual);
        }

        [Test()]
        public void Evaluate_FitnessLowerThanZero_Zero()
        {
            var target = new TspFitness(10, 0, 10000000, 0, 10000000);
            var chromosome = new TspChromosome(10);

            var actual = target.Evaluate(chromosome);
            ClassicAssert.AreEqual(0, actual);
        }
    }
}

