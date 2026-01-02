using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Mathematic
{
    [TestFixture()]
    [Category("Extensions")]
    public class FunctionBuilderChromosomeTest
    {
        [Test()]
        public void BuildAvailableOperations_ParametersCount_AvailableOperations()
        {
            var actual = FunctionBuilderChromosome.BuildAvailableOperations(4);
            ClassicAssert.AreEqual(10, actual.Count);

            ClassicAssert.AreEqual("", actual[0]);
            ClassicAssert.AreEqual("+", actual[1]);
            ClassicAssert.AreEqual("-", actual[2]);
            ClassicAssert.AreEqual("/", actual[3]);
            ClassicAssert.AreEqual("*", actual[4]);
            ClassicAssert.AreEqual("__INT__", actual[5]);
            ClassicAssert.AreEqual("A", actual[6]);
            ClassicAssert.AreEqual("B", actual[7]);
            ClassicAssert.AreEqual("C", actual[8]);
            ClassicAssert.AreEqual("D", actual[9]);
        }
    }
}