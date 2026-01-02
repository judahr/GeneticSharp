using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Crossovers
{
    [TestFixture()]
    [Category("Crossovers")]
    public class CrossoverServiceTest
    {
        [Test()]
        public void GetCrossoverTypes_NoArgs_AllAvailableCrossovers()
        {
            var actual = CrossoverService.GetCrossoverTypes();

            ClassicAssert.AreEqual(12, actual.Count);
            var index = -1;
            ClassicAssert.AreEqual(typeof(AlternatingPositionCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(CutAndSpliceCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(CycleCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(OnePointCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(OrderBasedCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(OrderedCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(PartiallyMappedCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(PositionBasedCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(ThreeParentCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(TwoPointCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(UniformCrossover), actual[++index]);
            ClassicAssert.AreEqual(typeof(VotingRecombinationCrossover), actual[++index]);
        }

        [Test()]
        public void GetCrossoverNames_NoArgs_AllAvailableCrossoversNames()
        {
            var actual = CrossoverService.GetCrossoverNames();

            ClassicAssert.AreEqual(12, actual.Count);
            var index = -1;
            ClassicAssert.AreEqual("Alternating-position (AP)", actual[++index]);
            ClassicAssert.AreEqual("Cut and Splice", actual[++index]);
            ClassicAssert.AreEqual("Cycle (CX)", actual[++index]);
            ClassicAssert.AreEqual("One-Point", actual[++index]);
            ClassicAssert.AreEqual("Order-based (OX2)", actual[++index]);
            ClassicAssert.AreEqual("Ordered (OX1)", actual[++index]);
            ClassicAssert.AreEqual("Partially Mapped (PMX)", actual[++index]);
            ClassicAssert.AreEqual("Position-based (POS)", actual[++index]);
            ClassicAssert.AreEqual("Three Parent", actual[++index]);
            ClassicAssert.AreEqual("Two-Point", actual[++index]);
            ClassicAssert.AreEqual("Uniform", actual[++index]);
            ClassicAssert.AreEqual("Voting Recombination (VR)", actual[++index]);
        }

        [Test()]
        public void CreateCrossoverByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                CrossoverService.CreateCrossoverByName("Test");
            }, "There is no ICrossover implementation with name 'Test'.");
        }

        [Test()]
        public void CreateCrossoverByName_ValidNameButInvalidConstructorArgs_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                CrossoverService.CreateCrossoverByName("One-Point", 1, 2, 3);
            }, "A ICrossover's implementation with name 'One-Point' was found, but seems the constructor args were invalid");
        }

        [Test()]
        public void CreateCrossoverByName_ValidName_CrossoverCreated()
        {
            ICrossover actual = CrossoverService.CreateCrossoverByName("One-Point", 1) as OnePointCrossover;
            ClassicAssert.IsNotNull(actual);

            actual = CrossoverService.CreateCrossoverByName("Ordered (OX1)") as OrderedCrossover;
            ClassicAssert.IsNotNull(actual);

            actual = CrossoverService.CreateCrossoverByName("Two-Point", 1, 2) as TwoPointCrossover;
            ClassicAssert.IsNotNull(actual);

            actual = CrossoverService.CreateCrossoverByName("Uniform", 1f) as UniformCrossover;
            ClassicAssert.IsNotNull(actual);

            actual = CrossoverService.CreateCrossoverByName("Partially Mapped (PMX)") as PartiallyMappedCrossover;
            ClassicAssert.IsNotNull(actual);

            actual = CrossoverService.CreateCrossoverByName("Three Parent") as ThreeParentCrossover;
            ClassicAssert.IsNotNull(actual);

            actual = CrossoverService.CreateCrossoverByName("Cycle (CX)") as CycleCrossover;
            ClassicAssert.IsNotNull(actual);
        }

        [Test()]
        public void GetCrossoverTypeByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                CrossoverService.GetCrossoverTypeByName("Test");
            }, "There is no ICrossover implementation with name 'Test'.");
        }

        [Test()]
        public void GetCrossoverTypeByName_ValidName_CrossoverTpe()
        {
            var actual = CrossoverService.GetCrossoverTypeByName("One-Point");
            ClassicAssert.AreEqual(typeof(OnePointCrossover), actual);

            actual = CrossoverService.GetCrossoverTypeByName("Ordered (OX1)");
            ClassicAssert.AreEqual(typeof(OrderedCrossover), actual);

            actual = CrossoverService.GetCrossoverTypeByName("Two-Point");
            ClassicAssert.AreEqual(typeof(TwoPointCrossover), actual);

            actual = CrossoverService.GetCrossoverTypeByName("Uniform");
            ClassicAssert.AreEqual(typeof(UniformCrossover), actual);

            actual = CrossoverService.GetCrossoverTypeByName("Partially Mapped (PMX)");
            ClassicAssert.AreEqual(typeof(PartiallyMappedCrossover), actual);

            actual = CrossoverService.GetCrossoverTypeByName("Three Parent");
            ClassicAssert.AreEqual(typeof(ThreeParentCrossover), actual);

            actual = CrossoverService.GetCrossoverTypeByName("Cycle (CX)");
            ClassicAssert.AreEqual(typeof(CycleCrossover), actual);
        }
    }
}