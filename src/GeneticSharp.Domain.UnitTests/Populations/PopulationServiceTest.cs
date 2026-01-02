using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Populations
{
    [TestFixture()]
    [Category("Populations")]
    public class PopulationServiceTest
    {
        [SetUp]
        public void Cleanup()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test()]
        public void GetGenerationStrategyTypes_NoArgs_AllAvailableIGenerationStrategy()
        {
            var actual = PopulationService.GetGenerationStrategyTypes();

            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual(typeof(PerformanceGenerationStrategy), actual[0]);
            ClassicAssert.AreEqual(typeof(TrackingGenerationStrategy), actual[1]);
        }

        [Test()]
        public void GetGenerationStrategyNames_NoArgs_AllAvailableGenerationStrategiesNames()
        {
            var actual = PopulationService.GetGenerationStrategyNames();

            ClassicAssert.AreEqual(2, actual.Count);
            ClassicAssert.AreEqual("Performance", actual[0]);
            ClassicAssert.AreEqual("Tracking", actual[1]);
        }

        [Test()]
        public void CreateGenerationStrategyByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                PopulationService.CreateGenerationStrategyByName("Test");
            }, "There is no IGenerationStrategy implementation with name 'Test'.");
        }

        [Test()]
        public void CreateGenerationStrategyByName_ValidNameButInvalidConstructorArgs_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                PopulationService.CreateGenerationStrategyByName("Tracking", 1);
            }, "A IGenerationStrategy's implementation with name 'Tracking' was found, but seems the constructor args were invalid.");
        }

        [Test()]
        public void CreateGenerationStrategyByName_ValidName_GenerationStrategyCreated()
        {
            IGenerationStrategy actual = PopulationService.CreateGenerationStrategyByName("Performance", 1) as PerformanceGenerationStrategy;
            ClassicAssert.IsNotNull(actual);

            actual = PopulationService.CreateGenerationStrategyByName("Tracking") as TrackingGenerationStrategy;
            ClassicAssert.IsNotNull(actual);
        }

        [Test()]
        public void GetGenerationStrategyTypeByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                PopulationService.GetGenerationStrategyTypeByName("Test");
            }, "There is no IGenerationStrategy implementation with name 'Test'.");
        }

        [Test()]
        public void GetGenerationStrategyTypeByName_ValidName_GenerationStrategyTpe()
        {
            var actual = PopulationService.GetGenerationStrategyTypeByName("Performance");
            ClassicAssert.AreEqual(typeof(PerformanceGenerationStrategy), actual);

            actual = PopulationService.GetGenerationStrategyTypeByName("Tracking");
            ClassicAssert.AreEqual(typeof(TrackingGenerationStrategy), actual);
        }
    }
}