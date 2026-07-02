using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using System;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture]
    [Category("Crossovers")]
    public class VotingRecombinationCrossoverTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void Constructor_ThresholdGreaterThanParentsNumber_Exception()
        {
            var actual = Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                new VotingRecombinationCrossover(2, 3);
            });

            StringAssert.StartsWith("The threshold should be smaller or equal to the parents number.", actual.Message);
        }

        [Test]
        public void Cross_DocumentationSample_Child()
        {
            var target = new VotingRecombinationCrossover(4, 3);

            // 1 4 3 5 2 6
            var chromosome1 = Substitute.ForPartsOf<ChromosomeBase>(6);
            chromosome1.ReplaceGenes(0, new Gene[] {
                new Gene(1),
                new Gene(4),
                new Gene(3),
                new Gene(5),
                new Gene(2),
                new Gene(6)
            });
            
            var child = Substitute.ForPartsOf<ChromosomeBase>(6);
            child.GenerateGene(2).Returns(new Gene(22));
            child.GenerateGene(3).Returns(new Gene(33));
            child.GenerateGene(4).Returns(new Gene(44));
            chromosome1.CreateNew().Returns(child);

            // 1 2 4 3 5 6
            var chromosome2 = Substitute.ForPartsOf<ChromosomeBase>(6);
            chromosome2.ReplaceGenes(0, new Gene[]
            {
                new Gene(1),
                new Gene(2),
                new Gene(4),
                new Gene(3),
                new Gene(5),
                new Gene(6)
            });

            // 3 2 1 5 4 6
            var chromosome3 = Substitute.ForPartsOf<ChromosomeBase>(6);
            chromosome3.ReplaceGenes(0, new Gene[]
            {
                new Gene(3),
                new Gene(2),
                new Gene(1),
                new Gene(5),
                new Gene(4),
                new Gene(6)
            });
      
            // 1 2 3 4 5 6
            var chromosome4 = Substitute.ForPartsOf<ChromosomeBase>(6);
            chromosome4.ReplaceGenes(0, new Gene[]
            {
                new Gene(1),
                new Gene(2),
                new Gene(3),
                new Gene(4),
                new Gene(5),
                new Gene(6)
            });
       
            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2, chromosome3, chromosome4 });

            ClassicAssert.AreEqual(1, actual.Count);
            var actualChild = actual[0];

            ClassicAssert.AreEqual(6, actualChild.Length);
         
            ClassicAssert.AreEqual(1, actualChild.GetGene(0).Value);
            ClassicAssert.AreEqual(2, actualChild.GetGene(1).Value);
            ClassicAssert.AreEqual(22, actualChild.GetGene(2).Value);
            ClassicAssert.AreEqual(33, actualChild.GetGene(3).Value);
            ClassicAssert.AreEqual(44, actualChild.GetGene(4).Value);
            ClassicAssert.AreEqual(6, actualChild.GetGene(5).Value);
        }

        /// <summary>
        /// Characterization test: pins today's tie-break behavior before replacing the per-gene
        /// GroupBy/OrderByDescending with a manual frequency count. When two distinct values both
        /// meet the threshold with an equal count, the value belonging to the first parent (in
        /// parent order) among the tied values must win.
        /// </summary>
        [Test]
        public void Cross_TiedFrequenciesAtThreshold_FirstEncounteredValueWins()
        {
            var target = new VotingRecombinationCrossover(4, 2);

            // Position 0: "B" (chromosome1, chromosome4) and "A" (chromosome2, chromosome3) both
            // occur exactly twice - tied, both meeting threshold=2. "B" is first-encountered
            // (from chromosome1), so it must win the tie.
            var chromosome1 = Substitute.ForPartsOf<ChromosomeBase>(2);
            chromosome1.ReplaceGenes(0, new Gene[] { new Gene("B"), new Gene("X") });

            var child = Substitute.ForPartsOf<ChromosomeBase>(2);
            chromosome1.CreateNew().Returns(child);

            var chromosome2 = Substitute.ForPartsOf<ChromosomeBase>(2);
            chromosome2.ReplaceGenes(0, new Gene[] { new Gene("A"), new Gene("X") });

            var chromosome3 = Substitute.ForPartsOf<ChromosomeBase>(2);
            chromosome3.ReplaceGenes(0, new Gene[] { new Gene("A"), new Gene("X") });

            var chromosome4 = Substitute.ForPartsOf<ChromosomeBase>(2);
            chromosome4.ReplaceGenes(0, new Gene[] { new Gene("B"), new Gene("X") });

            var actual = target.Cross(new List<IChromosome>() { chromosome1, chromosome2, chromosome3, chromosome4 });

            ClassicAssert.AreEqual(1, actual.Count);
            var actualChild = actual[0];

            ClassicAssert.AreEqual("B", actualChild.GetGene(0).Value);
            ClassicAssert.AreEqual("X", actualChild.GetGene(1).Value);
        }
    }
}