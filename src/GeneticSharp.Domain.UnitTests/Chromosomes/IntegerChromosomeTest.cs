using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Chromosomes
{
    [TestFixture]
    public class IntegerChromosomeTest
    {
        [Test]
        public void ToInteger_PositiveValue_Integer()
        {
            RandomizationProvider.Current = Substitute.For<IRandomization> ();
            RandomizationProvider.Current.GetInt (0, 3).Returns (2);

            var target = new IntegerChromosome (0, 3);
            ClassicAssert.AreEqual("00000000000000000000000000000010", target.ToString());

            var actual = target.ToInteger();
            ClassicAssert.AreEqual (2, actual);            
        }

        [Test]
        public void ToInteger_NegativeValue_Integer()
        {
            RandomizationProvider.Current = Substitute.For<IRandomization>();
            RandomizationProvider.Current.GetInt(0, 3).Returns(-2);

            var target = new IntegerChromosome(0, 3);
            ClassicAssert.AreEqual("11111111111111111111111111111110", target.ToString());

            var actual = target.ToInteger();
            ClassicAssert.AreEqual(-2, actual);
        }

        [Test]
        public void CreateNew_NoArgs_NewInstance()
        {
            RandomizationProvider.Current = Substitute.For<IRandomization>();
            RandomizationProvider.Current.GetInt(0, 3).Returns(-2);

            var old = new IntegerChromosome(0, 3);
            var target = old.CreateNew() as IntegerChromosome;

            ClassicAssert.AreNotSame(old, target);
            ClassicAssert.AreEqual("11111111111111111111111111111110", target.ToString());

            var actual = target.ToInteger();
            ClassicAssert.AreEqual(-2, actual);
        }

        [Test]
        public void FlipGene_Index_ValueFlip()
        {
            RandomizationProvider.Current = Substitute.For<IRandomization>();
            RandomizationProvider.Current.GetInt(0, 3).Returns(-2);

            var target = new IntegerChromosome(0, 3);
            ClassicAssert.AreEqual("11111111111111111111111111111110", target.ToString());

            target.FlipGene(0);
            ClassicAssert.AreEqual("01111111111111111111111111111110", target.ToString());

            target.FlipGene(1);
            ClassicAssert.AreEqual("00111111111111111111111111111110", target.ToString());

            target.FlipGene(10);
            ClassicAssert.AreEqual("00111111110111111111111111111110", target.ToString());

            target.FlipGene(31);
            ClassicAssert.AreEqual("00111111110111111111111111111111", target.ToString());

            target.FlipGene(30);
            ClassicAssert.AreEqual("00111111110111111111111111111101", target.ToString());
        }
    }
}

