using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Reinsertions
{
    [TestFixture()]
    [Category("Reinsertions")]
    public class ElitistReinsertionTest
    {
        [Test()]
        public void SelectChromosomes_offspringSizeLowerThanMinSize_Selectoffspring()
        {
            var target = new ElitistReinsertion();

            var population = new Population(6, 8, Substitute.For<ChromosomeBase>(2));
            var offspring = new List<IChromosome>() {
                Substitute.For<ChromosomeBase> (2),
                Substitute.For<ChromosomeBase> (2),
                Substitute.For<ChromosomeBase> (3),
                Substitute.For<ChromosomeBase> (4)
            };

            var parents = new List<IChromosome>() {
                Substitute.For<ChromosomeBase> (5),
                Substitute.For<ChromosomeBase> (6),
                Substitute.For<ChromosomeBase> (7),
                Substitute.For<ChromosomeBase> (8)
            };

            parents[0].Fitness = 0.2;
            parents[1].Fitness = 0.3;
            parents[2].Fitness = 0.5;
            parents[3].Fitness = 0.7;

            var selected = target.SelectChromosomes(population, offspring, parents);
            ClassicAssert.AreEqual(6, selected.Count);
            ClassicAssert.AreEqual(2, selected[0].Length);
            ClassicAssert.AreEqual(2, selected[1].Length);
            ClassicAssert.AreEqual(3, selected[2].Length);
            ClassicAssert.AreEqual(4, selected[3].Length);
            ClassicAssert.AreEqual(8, selected[4].Length);
            ClassicAssert.AreEqual(7, selected[5].Length);
        }
    }
}

