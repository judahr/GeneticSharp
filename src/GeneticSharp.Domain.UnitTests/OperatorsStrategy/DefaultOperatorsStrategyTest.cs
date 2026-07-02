using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.OperatorsStrategy
{
    [TestFixture()]
    [Category("OperatorsStrategy")]
    public class DefaultOperatorsStrategyTest
    {
        [TearDown]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        /// <summary>
        /// Characterization test: pins today's parent-selection indexing in SelectParentsAndCross
        /// before replacing its Skip().Take().ToList() with direct indexing. With 5 parents,
        /// ParentsNumber = 2 and population.MinSize = 6, the loop in DefaultOperatorsStrategy.Cross
        /// visits firstParentIndex 0, 2 and 4: the first two iterations must cross exactly
        /// [parents[0], parents[1]] and [parents[2], parents[3]]; the last iteration only has
        /// parents[4] left over (fewer than ParentsNumber), so it must not cross at all.
        /// </summary>
        [Test]
        public void Cross_MoreParentsIndexesThanAvailable_CrossesPairsAndSkipsRemainder()
        {
            RandomizationProvider.Current = new BasicRandomization();

            var c0 = Substitute.ForPartsOf<ChromosomeBase>(2);
            var c1 = Substitute.ForPartsOf<ChromosomeBase>(2);
            var c2 = Substitute.ForPartsOf<ChromosomeBase>(2);
            var c3 = Substitute.ForPartsOf<ChromosomeBase>(2);
            var c4 = Substitute.ForPartsOf<ChromosomeBase>(2);
            var parents = new List<IChromosome>() { c0, c1, c2, c3, c4 };

            var population = Substitute.For<IPopulation>();
            population.MinSize.Returns(6);

            var crossover = Substitute.For<ICrossover>();
            crossover.ParentsNumber.Returns(2);
            crossover.Cross(Arg.Any<IList<IChromosome>>()).Returns(new List<IChromosome>());

            var target = new DefaultOperatorsStrategy();

            // crossoverProbability = 1f always satisfies "GetDouble() <= crossoverProbability",
            // since GetDouble() never returns a value >= 1.
            target.Cross(population, crossover, 1f, parents);

            // ChromosomeBase overrides Equals/== to compare by fitness (all null here, so every
            // instance would appear "equal" to every other), so identity must be checked with
            // ReferenceEquals rather than == or List<T>.Contains.
            crossover.Received(1).Cross(Arg.Is<IList<IChromosome>>(l =>
                l.Count == 2 && ReferenceEquals(l[0], c0) && ReferenceEquals(l[1], c1)));
            crossover.Received(1).Cross(Arg.Is<IList<IChromosome>>(l =>
                l.Count == 2 && ReferenceEquals(l[0], c2) && ReferenceEquals(l[1], c3)));
            crossover.Received(0).Cross(Arg.Is<IList<IChromosome>>(l =>
                l.Any(x => ReferenceEquals(x, c4))));
            crossover.Received(2).Cross(Arg.Any<IList<IChromosome>>());
        }
    }
}
