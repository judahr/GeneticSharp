using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Populations
{
    [TestFixture]
    [Category("Populations")]
    public class PerformanceGenerationStrategyTest
    {
        [SetUp]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test]
        public void RegisterNewGeneration_GenerationDoesNotExceedGenerationsNumber_DoNothing()
        {
            var target = new PerformanceGenerationStrategy(4);
            var population = new Population(2, 6, new ChromosomeStub());

            population.CreateInitialGeneration();
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(1, population.Generations.Count);

            population.CreateNewGeneration(new List<IChromosome>() { new ChromosomeStub(), new ChromosomeStub() });
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(2, population.Generations.Count);

            population.CreateNewGeneration(new List<IChromosome>() { new ChromosomeStub(), new ChromosomeStub() });
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(3, population.Generations.Count);

            population.CreateNewGeneration(new List<IChromosome>() { new ChromosomeStub(), new ChromosomeStub() });
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(4, population.Generations.Count);
        }

        [Test]
        public void RegisterNewGeneration_GenerationExceedGenerationsNumber_RemoveOldOne()
        {
            var target = new PerformanceGenerationStrategy();
            var population = new Population(2, 6, new ChromosomeStub());

            population.CreateInitialGeneration();
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(1, population.Generations.Count);

            population.CreateNewGeneration(new List<IChromosome>() { new ChromosomeStub(), new ChromosomeStub() });
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(1, population.Generations.Count);

            population.CreateNewGeneration(new List<IChromosome>() { new ChromosomeStub(), new ChromosomeStub() });
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(1, population.Generations.Count);

            population.CreateNewGeneration(new List<IChromosome>() { new ChromosomeStub(), new ChromosomeStub() });
            target.RegisterNewGeneration(population);
            ClassicAssert.AreEqual(1, population.Generations.Count);
        }
    }
}