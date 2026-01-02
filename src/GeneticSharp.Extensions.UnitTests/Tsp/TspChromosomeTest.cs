using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Tsp
{
    [TestFixture()]
    [Category("Extensions")]
    public class TspChromosomeTest
    {
        [Test()]
        public void Constructor_OneCity_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                new TspChromosome(1);
            }, "The minimum length for a chromosome is 2 genes.");
        }

        [Test()]
        public void GenerateGene_FitnessLowerThanZero_Zero()
        {
            var target = new TspChromosome(10);
            var cityIndex = Convert.ToDouble(target.GenerateGene(0).Value);
            ClassicAssert.IsTrue(cityIndex >= 0 && cityIndex < 10);
        }

        [Test()]
        public void Clone_NoArgs_Cloned()
        {
            var target = new TspChromosome(10);
         
            var actual = target.Clone() as TspChromosome;
            ClassicAssert.IsFalse(Object.ReferenceEquals(target, actual));
        }
    }
}

