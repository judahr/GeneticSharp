using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Selections
{
    [TestFixture()]
    [Category("Selections")]
    public class EliteSelectionTest
    {
        [Test]
        public void SelectChromosomes_InvalidNumber_Exception()
        {
            var target = new EliteSelection();

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.SelectChromosomes(-1, null);
            }, "The number of selected chromosomes should be at least 2.");

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.SelectChromosomes(0, null);
            }, "The number of selected chromosomes should be at least 2.");

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.SelectChromosomes(1, null);
            }, "The number of selected chromosomes should be at least 2.");
        }

        [Test]
        public void SelectChromosomes_NullGeneration_Exception()
        {
            var target = new EliteSelection();

            var actual = Assert.Catch<ArgumentNullException>(() =>
            {
                target.SelectChromosomes(2, null);
            });

            ClassicAssert.AreEqual("generation", actual.ParamName);
        }

        [Test]
        public void SelectChromosomes_Generation_ChromosomesSelected()
        {
            var target = new EliteSelection();
            var c1 = Substitute.ForPartsOf<ChromosomeBase>(2);
            c1.Fitness = 0.1;

            var c2 = Substitute.ForPartsOf<ChromosomeBase>(2);
            c2.Fitness = 0.5;

            var c3 = Substitute.ForPartsOf<ChromosomeBase>(2);
            c3.Fitness = 0;

            var c4 = Substitute.ForPartsOf<ChromosomeBase>(2);
            c4.Fitness = 0.7;

            var generation1 = new Generation(1, new List<IChromosome>() {
                c1, c2, c3
            });

            var generation2 = new Generation(1, new List<IChromosome>() {
                c1, c2, c3, c4
            });


            var actual = target.SelectChromosomes(2, generation1);
            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(0.5, actual[0].Fitness);
            ClassicAssert.AreEqual(0.1, actual[1].Fitness);

            actual = target.SelectChromosomes(3, generation2);
            ClassicAssert.AreEqual(3, actual.Count);
            ClassicAssert.AreEqual(0.7, actual[0].Fitness);
            ClassicAssert.AreEqual(0.5, actual[1].Fitness);
            ClassicAssert.AreEqual(0.1, actual[2].Fitness);
        }

        /// <summary>
        /// https://github.com/giacomelli/GeneticSharp/issues/72
        /// </summary>
        [Test]
        public void SelectChromosomes_Issue72_Solved()
        {
            var target = new EliteSelection();
            var chromosomes = new IChromosome[10];

            for (int i = 0; i < chromosomes.Length; i++)
            {                
                var c = Substitute.ForPartsOf<ChromosomeBase>(2);
                c.Fitness = i;
                chromosomes[i] = c;
            }

            var generation3 = new Generation(3, chromosomes.Take(4).ToList());            
            var generation2 = new Generation(2, chromosomes.Skip(4).Take(3).ToList());
            var generation1 = new Generation(1, chromosomes.Skip(7).Take(3).ToList());


            var actual = target.SelectChromosomes(2, generation1);
            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(9, actual[0].Fitness);
            ClassicAssert.AreEqual(8, actual[1].Fitness);

            actual = target.SelectChromosomes(3, generation2);
            ClassicAssert.AreEqual(3, actual.Count);
            ClassicAssert.AreEqual(9, actual[0].Fitness);
            ClassicAssert.AreEqual(6, actual[1].Fitness);
            ClassicAssert.AreEqual(5, actual[2].Fitness);            

            actual = target.SelectChromosomes(2, generation3);
            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(9, actual[0].Fitness);
            ClassicAssert.AreEqual(3, actual[1].Fitness);
        }
    }
}

