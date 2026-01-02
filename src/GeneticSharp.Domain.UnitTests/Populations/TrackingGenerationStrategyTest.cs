using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Populations
{
    [TestFixture]
    [Category("Populations")]
    public class TrackingGenerationStrategyTest
    {
        [Test]
        public void RegisterNewGeneration_AnyGeneration_DoNothing()
        {
            var target = new TrackingGenerationStrategy();
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
    }
}