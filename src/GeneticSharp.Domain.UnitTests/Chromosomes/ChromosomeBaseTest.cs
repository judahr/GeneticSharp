using System;
using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Chromosomes
{
    [Category("Chromosomes")]
    [TestFixture]
    public class ChromosomeBaseTest
    {
        [Test]
        public void Constructor_InvalidLength_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                try
                {
                    Substitute.For<ChromosomeBase>(1);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }, "The minimum length for a chromosome is 2 genes.");
        }

        [Test]
        public void CompareTo_Others_DiffResults()
        {
            var target = Substitute.For<ChromosomeBase>(2);
            target.Fitness = 0.5;

            var other = Substitute.For<ChromosomeBase>(2);
            other.Fitness = 0.5;

            ClassicAssert.AreEqual(-1, target.CompareTo(null));
            ClassicAssert.AreEqual(0, target.CompareTo(other));

            other.Fitness = 0.4;
            ClassicAssert.AreEqual(1, target.CompareTo(other));

            other.Fitness = 0.6;
            ClassicAssert.AreEqual(-1, target.CompareTo(other));
        }

        [Test]
        public void Fitness_AnyChange_Null()
        {
            var target = Substitute.For<ChromosomeBase>(2);
            ClassicAssert.IsFalse(target.Fitness.HasValue);
            target.Fitness = 0.5;
            ClassicAssert.IsTrue(target.Fitness.HasValue);

            target.Fitness = 0.5;
            target.ReplaceGene(0, new Gene(0));
            ClassicAssert.IsFalse(target.Fitness.HasValue);

            target.Fitness = 0.5;
            target.ReplaceGenes(0, new Gene[] { new Gene(0) });
            ClassicAssert.IsFalse(target.Fitness.HasValue);

            target.Fitness = 0.5;
            target.GenerateGene(0);
            ClassicAssert.IsTrue(target.Fitness.HasValue);

            target.Fitness = 0.5;
            target.ReplaceGene(0, new Gene(0));
            ClassicAssert.IsFalse(target.Fitness.HasValue);
        }

        [Test]
        public void ReplaceGene_InvalidIndex_Exception()
        {
            var target = Substitute.For<ChromosomeBase>(2);

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.ReplaceGene(2, new Gene(0));
            }, "There is no Gene on index 2 to be replaced.");

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.ReplaceGene(3, new Gene(0));
            }, "There is no Gene on index 3 to be replaced.");
        }

        [Test]
        public void ReplaceGene_ValidIndex_Replaced()
        {
            var target = Substitute.For<ChromosomeBase>(2);

            target.ReplaceGene(0, new Gene(2));
            target.ReplaceGene(1, new Gene(6));

            var actual = target.GetGenes();
            ClassicAssert.AreEqual(2, actual.Length);
            ClassicAssert.AreEqual(2, actual[0].Value);
            ClassicAssert.AreEqual(6, actual[1].Value);
        }

        [Test]
        public void ReplaceGenes_InvalidIndex_Exception()
        {
            var target = Substitute.For<ChromosomeBase>(2);

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.ReplaceGenes(2, new Gene[] { new Gene() });
            }, "There is no Gene on index 2 to be replaced.");

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.ReplaceGenes(3, new Gene[] { new Gene() });
            }, "There is no Gene on index 3 to be replaced.");
        }

        [Test]
        public void ReplaceGenes_NullGenes_Exception()
        {
            var target = Substitute.For<ChromosomeBase>(2);

            Assert.Catch<ArgumentNullException>(() =>
            {
                target.ReplaceGenes(0, null);
            });
        }

        [Test]
        public void ReplaceGenes_ValidIndex_Replaced()
        {
            var target = Substitute.For<ChromosomeBase>(4);

            target.ReplaceGenes(0, new Gene[] { new Gene(1), new Gene(2) });

            var actual = target.GetGenes();
            ClassicAssert.AreEqual(4, actual.Length);
            ClassicAssert.AreEqual(1, actual[0].Value);
            ClassicAssert.AreEqual(2, actual[1].Value);

            target.ReplaceGenes(2, new Gene[] { new Gene(3), new Gene(4) });

            actual = target.GetGenes();
            ClassicAssert.AreEqual(4, actual.Length);
            ClassicAssert.AreEqual(1, actual[0].Value);
            ClassicAssert.AreEqual(2, actual[1].Value);
            ClassicAssert.AreEqual(3, actual[2].Value);
            ClassicAssert.AreEqual(4, actual[3].Value);

            target.ReplaceGenes(3, new Gene[] { new Gene(5) });

            actual = target.GetGenes();
            ClassicAssert.AreEqual(4, actual.Length);
            ClassicAssert.AreEqual(1, actual[0].Value);
            ClassicAssert.AreEqual(2, actual[1].Value);
            ClassicAssert.AreEqual(3, actual[2].Value);
            ClassicAssert.AreEqual(5, actual[3].Value);
        }

        [Test]
        public void Resize_InvalidLength_Exception()
        {
            var target = Substitute.For<ChromosomeBase>(2);

            Assert.Catch<ArgumentException>(() =>
            {
                target.Resize(1);
            }, "The minimum length for a chromosome is 2 genes.");
        }

        [Test]
        public void Resize_ToLowerLength_TruncateGenes()
        {
            var target = Substitute.For<ChromosomeBase>(4);
            target.ReplaceGenes(0, new Gene[]
            {
                new Gene(1),
                new Gene(2),
                new Gene(3),
                new Gene(4)
            });

            target.Resize(2);
            ClassicAssert.AreEqual(2, target.Length);
            ClassicAssert.AreEqual(1, target.GetGene(0).Value);
            ClassicAssert.AreEqual(2, target.GetGene(1).Value);
        }

        [Test]
        public void Resize_ToGreaterLength_KeepOldGenesAndNullValueNewOnes()
        {
            var target = Substitute.For<ChromosomeBase>(2);
            target.ReplaceGenes(0, new Gene[]
            {
                new Gene(1),
                new Gene(2)
            });

            target.Resize(4);
            ClassicAssert.AreEqual(4, target.Length);
            ClassicAssert.AreEqual(1, target.GetGene(0).Value);
            ClassicAssert.AreEqual(2, target.GetGene(1).Value);
            ClassicAssert.IsNull(target.GetGene(2).Value);
            ClassicAssert.IsNull(target.GetGene(3).Value);
        }

        [Test]
        public void Equals_NotChromosome_False()
        {
            var target = new ChromosomeStub();
            ClassicAssert.IsFalse(target.Equals(1));
        }

        [Test]
        public void GetHashCode_NoFitness_Zero()
        {
            var target = new ChromosomeStub();
            target.Fitness = null;
            ClassicAssert.AreEqual(0, target.GetHashCode());
        }

        [Test]
        public void GetHashCode_Fitness_EqualsFitnessHashCode()
        {
            var target = new ChromosomeStub();
            target.Fitness = 123;
            ClassicAssert.AreEqual(target.Fitness.GetHashCode(), target.GetHashCode());
        }

        [Test]
        public void OperatorEquals_EqualsReferences_True()
        {
            var first = new ChromosomeStub(1.0);
            var second = first;

            ClassicAssert.IsTrue(first == second);
        }

        [Test]
        public void OperatorEquals_AnyNull_False()
        {
            var first = new ChromosomeStub(1.0);
            var second = new ChromosomeStub(1.0);

            ClassicAssert.IsFalse(null == second);
            ClassicAssert.IsFalse(first == null);
        }

        [Test]
        public void OperatorEquals_Diff_False()
        {
            var first = new ChromosomeStub(1.0);
            var second = new ChromosomeStub(2.0);

            ClassicAssert.IsFalse(first == second);
        }

        [Test]
        public void OperatorEquals_Equals_True()
        {
            var first = new ChromosomeStub(1.0);
            var second = new ChromosomeStub(1.0);

            ClassicAssert.IsTrue(first == second);
        }

        [Test]
        public void OperatorDiff_Equals_False()
        {
            var first = new ChromosomeStub(1.0);
            var second = new ChromosomeStub(1.0);

            ClassicAssert.IsFalse(first != second);
        }

        [Test]
        public void OperatorDiff_Diff_True()
        {
            var first = new ChromosomeStub(1.0);
            var second = new ChromosomeStub(2.0);

            ClassicAssert.IsTrue(first != second);
        }

        [Test]
        public void OperatorLowerThan_ReferenceEquals_False()
        {
            var first = new ChromosomeStub(1.0);
            var second = first;

            ClassicAssert.IsFalse(first < second);
        }

        [Test]
        public void OperatorLowerThan_FirstNull_True()
        {
            var second = new ChromosomeStub(2.0);

            ClassicAssert.IsTrue(null < second);
        }

        [Test]
        public void OperatorLowerThan_SecondNull_False()
        {
            var first = new ChromosomeStub(2.0);

            ClassicAssert.IsFalse(first < null);
        }

        [Test]
        public void OperatorLowerThan_FitnessGreater_False()
        {
            var first = new ChromosomeStub(3.0);
            var second = new ChromosomeStub(2.0);

            ClassicAssert.IsFalse(first < second);
        }

        [Test]
        public void OperatorLowerThan_FitnessLower_True()
        {
            var first = new ChromosomeStub(1.0);
            var second = new ChromosomeStub(2.0);

            ClassicAssert.IsTrue(first < second);
        }

        [Test]
        public void OperatorGreaterThan_DiffAndFitnessGreater_True()
        {
            var first = new ChromosomeStub(3.0);
            var second = new ChromosomeStub(2.0);

            ClassicAssert.IsTrue(first > second);
            ClassicAssert.IsFalse(second > first);
        }

        [Test]
        public void OperatorGreaterThan_Equals_False()
        {
            var first = new ChromosomeStub(2.0);
            var second = new ChromosomeStub(2.0);

            ClassicAssert.IsFalse(first > second);
            ClassicAssert.IsFalse(second > first);
        }
    }
}