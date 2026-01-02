using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Ghostwriter
{
    [TestFixture]
    [Category("Extensions")]
    class GhostwriterTest
    {
        [SetUp]
        public void InitializeTest()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test()]
        public void Evolve_ManyGenerations_Fast()
        {
            var selection = new EliteSelection();
            var crossover = new UniformCrossover();
            var mutation = new UniformMutation(true);
            var chromosome = new GhostwriterChromosome(4, new string[] { "The", "C#", "Genetic", "Algorithm", "library" });
            var fitness = new GhostwriterFitness((t) => t.Length);
            
            var population = new Population(10, 10, chromosome);
            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new GenerationNumberTermination(5);
            ga.Start();

            ClassicAssert.NotNull(ga.BestChromosome);            
        }

        [Test]
        public void Clone_Chromosome_Cloned()
        {
            var target = new GhostwriterChromosome(2, new string[] { "a", "b", "c" });
            var actual = target.Clone() as GhostwriterChromosome;
            ClassicAssert.AreEqual(2, actual.Length);
            ClassicAssert.AreEqual(target.BuildText(), actual.BuildText());
        }
    }
}
