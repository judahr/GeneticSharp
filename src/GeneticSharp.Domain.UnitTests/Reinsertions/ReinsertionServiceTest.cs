using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Reinsertions
{
    [TestFixture()]
    [Category("Reinsertions")]
    public class ReinsertionServiceTest
    {
        [Test()]
        public void GetReinsertionTypes_NoArgs_AllAvailableReinsertions()
        {
            var actual = ReinsertionService.GetReinsertionTypes();

            ClassicAssert.AreEqual(4, actual.Count);
            ClassicAssert.AreEqual(typeof(ElitistReinsertion), actual[0]);
            ClassicAssert.AreEqual(typeof(FitnessBasedReinsertion), actual[1]);
            ClassicAssert.AreEqual(typeof(PureReinsertion), actual[2]);
            ClassicAssert.AreEqual(typeof(UniformReinsertion), actual[3]);
        }

        [Test()]
        public void GetReinsertionNames_NoArgs_AllAvailableReinsertionsNames()
        {
            var actual = ReinsertionService.GetReinsertionNames();

            ClassicAssert.AreEqual(4, actual.Count);
            ClassicAssert.AreEqual("Elitist", actual[0]);
            ClassicAssert.AreEqual("Fitness Based", actual[1]);
            ClassicAssert.AreEqual("Pure", actual[2]);
            ClassicAssert.AreEqual("Uniform", actual[3]);
        }

        [Test()]
        public void CreateReinsertionByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                ReinsertionService.CreateReinsertionByName("Test");
            }, "There is no IReinsertion implementation with name 'Test'.");
        }

        [Test()]
        public void CreateReinsertionByName_ValidNameButInvalidConstructorArgs_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                ReinsertionService.CreateReinsertionByName("Elitist", 1, 2, 3);
            }, "A IReinsertion's implementation with name 'Elitist' was found, but seems the constructor args were invalid.");
        }

        [Test()]
        public void CreateReinsertionByName_ValidName_ReinsertionCreated()
        {
            IReinsertion actual = ReinsertionService.CreateReinsertionByName("Elitist") as ElitistReinsertion;
            ClassicAssert.IsNotNull(actual);

            actual = ReinsertionService.CreateReinsertionByName("Fitness Based") as FitnessBasedReinsertion;
            ClassicAssert.IsNotNull(actual);

            actual = ReinsertionService.CreateReinsertionByName("Pure") as PureReinsertion;
            ClassicAssert.IsNotNull(actual);

            actual = ReinsertionService.CreateReinsertionByName("Uniform") as UniformReinsertion;
            ClassicAssert.IsNotNull(actual);
        }

        [Test()]
        public void GetReinsertionTypeByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                ReinsertionService.GetReinsertionTypeByName("Test");
            }, "There is no IReinsertion implementation with name 'Test'.");
        }

        [Test()]
        public void GetReinsertionTypeByName_ValidName_ReinsertionTpe()
        {
            var actual = ReinsertionService.GetReinsertionTypeByName("Elitist");
            ClassicAssert.AreEqual(typeof(ElitistReinsertion), actual);

            actual = ReinsertionService.GetReinsertionTypeByName("Fitness Based");
            ClassicAssert.AreEqual(typeof(FitnessBasedReinsertion), actual);

            actual = ReinsertionService.GetReinsertionTypeByName("Pure");
            ClassicAssert.AreEqual(typeof(PureReinsertion), actual);

            actual = ReinsertionService.GetReinsertionTypeByName("Uniform");
            ClassicAssert.AreEqual(typeof(UniformReinsertion), actual);
        }
    }
}