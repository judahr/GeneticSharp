using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Terminations
{
    [TestFixture()]
    [Category("Terminations")]
    public class TerminationServiceTest
    {
        [Test()]
        public void GetTerminationTypes_NoArgs_AllAvailableTerminations()
        {
            var actual = TerminationService.GetTerminationTypes();

            ClassicAssert.AreEqual(6, actual.Count);
            ClassicAssert.AreEqual(typeof(AndTermination), actual[0]);
            ClassicAssert.AreEqual(typeof(FitnessStagnationTermination), actual[1]);
            ClassicAssert.AreEqual(typeof(FitnessThresholdTermination), actual[2]);
            ClassicAssert.AreEqual(typeof(GenerationNumberTermination), actual[3]);
            ClassicAssert.AreEqual(typeof(OrTermination), actual[4]);
            ClassicAssert.AreEqual(typeof(TimeEvolvingTermination), actual[5]);
        }

        [Test()]
        public void GetTerminationNames_NoArgs_AllAvailableTerminationsNames()
        {
            var actual = TerminationService.GetTerminationNames();

            ClassicAssert.AreEqual(6, actual.Count);
            ClassicAssert.AreEqual("And", actual[0]);
            ClassicAssert.AreEqual("Fitness Stagnation", actual[1]);
            ClassicAssert.AreEqual("Fitness Threshold", actual[2]);
            ClassicAssert.AreEqual("Generation Number", actual[3]);
            ClassicAssert.AreEqual("Or", actual[4]);
            ClassicAssert.AreEqual("Time Evolving", actual[5]);

        }

        [Test()]
        public void CreateTerminationByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                TerminationService.CreateTerminationByName("Test");
            }, "There is no ITermination implementation with name 'Test'.");
        }

        [Test()]
        public void CreateTerminationByName_ValidNameButInvalidConstructorArgs_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                TerminationService.CreateTerminationByName("Generation Number", 1f);
            }, "A ITermination's implementation with name 'Generation Number' was found, but seems the constructor args were invalid.");
        }

        [Test()]
        public void CreateTerminationByName_ValidName_TerminationCreated()
        {
            ITermination actual = TerminationService.CreateTerminationByName("Fitness Stagnation") as FitnessStagnationTermination;
            ClassicAssert.IsNotNull(actual);

            actual = TerminationService.CreateTerminationByName("Fitness Threshold") as FitnessThresholdTermination;
            ClassicAssert.IsNotNull(actual);

            actual = TerminationService.CreateTerminationByName("Generation Number") as GenerationNumberTermination;
            ClassicAssert.IsNotNull(actual);

            actual = TerminationService.CreateTerminationByName("Time Evolving") as TimeEvolvingTermination;
            ClassicAssert.IsNotNull(actual);
        }

        [Test()]
        public void GetTerminationTypeByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                TerminationService.GetTerminationTypeByName("Test");
            }, "There is no ITermination implementation with name 'Test'.");
        }

        [Test()]
        public void GetTerminationTypeByName_ValidName_CrossoverTpe()
        {
            var actual = TerminationService.GetTerminationTypeByName("Generation Number");
            ClassicAssert.AreEqual(typeof(GenerationNumberTermination), actual);

            actual = TerminationService.GetTerminationTypeByName("Time Evolving");
            ClassicAssert.AreEqual(typeof(TimeEvolvingTermination), actual);

            actual = TerminationService.GetTerminationTypeByName("Fitness Threshold");
            ClassicAssert.AreEqual(typeof(FitnessThresholdTermination), actual);

            actual = TerminationService.GetTerminationTypeByName("Fitness Stagnation");
            ClassicAssert.AreEqual(typeof(FitnessStagnationTermination), actual);
        }
    }
}