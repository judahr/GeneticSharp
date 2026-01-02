using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Legacy;
namespace GeneticSharp.Domain.UnitTests.Terminations
{
    [TestFixture()]
    [Category("Terminations")]
    public class FitnessThresholdTerminationTest
    {
        [Test()]
        public void HasReached_BestChromosomeHasLowerFitness_False()
        {
            var ga = Substitute.For<IGeneticAlgorithm>();

            ga.BestChromosome.Returns(new ChromosomeStub() { Fitness = 0.4 },
                                      new ChromosomeStub() { Fitness = 0.499 });

            var target = new FitnessThresholdTermination(0.5);
            ClassicAssert.IsFalse(target.HasReached(ga));
            ClassicAssert.IsFalse(target.HasReached(ga));
        }

        [Test()]
        public void HasReached_BestChromosomeHasGreaterOrEqualFitness_True()
        {
            var ga = Substitute.For<IGeneticAlgorithm>();

            ga.BestChromosome.Returns(new ChromosomeStub() { Fitness = 0.4 },
                                      new ChromosomeStub() { Fitness = 0.8 });

            var target = new FitnessThresholdTermination(0.8);

            ClassicAssert.IsFalse(target.HasReached(ga));
            ClassicAssert.IsTrue(target.HasReached(ga));
        }
    }
}

