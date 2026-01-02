using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Checkers
{
    [TestFixture]
    [Category("Extensions")]
    public class CheckersChromosomeTest
    {
        [Test()]
        public void Clone_NoArgs_Cloned()
        {
            var target = new CheckersChromosome(2, 10);

            var actual = target.Clone() as CheckersChromosome;
            ClassicAssert.IsFalse(Object.ReferenceEquals(target, actual));
            ClassicAssert.AreEqual(2, actual.Moves.Count);
        }
    }
}