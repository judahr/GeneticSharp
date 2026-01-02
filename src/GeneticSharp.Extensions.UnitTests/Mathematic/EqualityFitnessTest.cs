using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Mathematic
{
    [TestFixture()]
    [Category("Extensions")]
    public class EqualityFitnessTest
    {
        [Test()]
        public void Evaluate_DiffChromosomes_DiffFitness()
        {
            var target = new EqualityFitness();

            var chromosome = new EquationChromosome(30, 4);
            chromosome.ReplaceGene(0, new Gene(0));
            chromosome.ReplaceGene(1, new Gene(7));
            chromosome.ReplaceGene(2, new Gene(-43));
            chromosome.ReplaceGene(3, new Gene(32));

            var actual = target.Evaluate(chromosome);
            ClassicAssert.Less(actual, 0);

            chromosome = new EquationChromosome(30, 4);
            chromosome.ReplaceGene(0, new Gene(17));
            chromosome.ReplaceGene(1, new Gene(7));
            chromosome.ReplaceGene(2, new Gene(-43));
            chromosome.ReplaceGene(3, new Gene(32));

            actual = target.Evaluate(chromosome); 
            ClassicAssert.AreEqual(0, actual);
        }
    }
}