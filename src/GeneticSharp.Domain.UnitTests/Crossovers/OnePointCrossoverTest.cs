using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture]
    [Category("Crossovers")]
    public class OnePointCrossoverTest
    {
        [Test]
        public void Cross_LessGenesThenSwapPoint_Exception()
        {
            var target = new OnePointCrossover(1);
            var chromosome1 = Substitute.For<ChromosomeBase>(2);
            var chromosome2 = Substitute.For<ChromosomeBase>(2);

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.Cross(new List<IChromosome>() {
                    chromosome1,
                    chromosome2
                });
            }, "The swap point index is 1, but there is only 2 genes. The swap should result at least one gene to each side.");
        }

        [Test]
        public void Cross_ParentsWithTwoGenes_Cross()
        {
            var target = new OnePointCrossover(0);
            var chromosome1 = Substitute.For<ChromosomeBase>(2);
            chromosome1.ReplaceGenes(0, new Gene[]
            {
                new Gene(1),
                new Gene(2)
            });
            chromosome1.CreateNew().Returns(Substitute.For<ChromosomeBase>(2));

            var chromosome2 = Substitute.For<ChromosomeBase>(2);
            chromosome2.ReplaceGenes(0, new Gene[]
            {
                new Gene(3),
                new Gene(4)
            });
            chromosome2.CreateNew().Returns(Substitute.For<ChromosomeBase>(2));

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2 });

            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(2, actual[0].Length);
            ClassicAssert.AreEqual(2, actual[1].Length);

            ClassicAssert.AreEqual(1, actual[0].GetGene(0).Value);
            ClassicAssert.AreEqual(4, actual[0].GetGene(1).Value);

            ClassicAssert.AreEqual(3, actual[1].GetGene(0).Value);
            ClassicAssert.AreEqual(2, actual[1].GetGene(1).Value);
        }
    }
}