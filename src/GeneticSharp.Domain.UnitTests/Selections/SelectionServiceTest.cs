using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Selections
{
    [TestFixture()]
    [Category("Selections")]
    public class SelectionServiceTest
    {
        [Test()]
        public void GetSelectionTypes_NoArgs_AllAvailableSelections()
        {
            var actual = SelectionService.GetSelectionTypes();

            ClassicAssert.AreEqual(6, actual.Count);
            ClassicAssert.AreEqual(typeof(EliteSelection), actual[0]);
            ClassicAssert.AreEqual(typeof(RankSelection), actual[1]);
            ClassicAssert.AreEqual(typeof(RouletteWheelSelection), actual[2]);
            ClassicAssert.AreEqual(typeof(StochasticUniversalSamplingSelection), actual[3]);
            ClassicAssert.AreEqual(typeof(TournamentSelection), actual[4]);
            ClassicAssert.AreEqual(typeof(TruncationSelection), actual[5]);
        }

        [Test()]
        public void GetSelectionNames_NoArgs_AllAvailableSelectionsNames()
        {
            var actual = SelectionService.GetSelectionNames();

            ClassicAssert.AreEqual(6, actual.Count);
            ClassicAssert.AreEqual("Elite", actual[0]);
            ClassicAssert.AreEqual("Rank", actual[1]);
            ClassicAssert.AreEqual("Roulette Wheel", actual[2]);
            ClassicAssert.AreEqual("Stochastic Universal Sampling", actual[3]);
            ClassicAssert.AreEqual("Tournament", actual[4]);
            ClassicAssert.AreEqual("Truncation", actual[5]);
        }

        [Test()]
        public void CreateSelectionByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                SelectionService.CreateSelectionByName("Test");
            }, "There is no ISelection implementation with name 'Test'.");
        }

        [Test()]
        public void CreateSelectionByName_ValidNameButInvalidConstructorArgs_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                SelectionService.CreateSelectionByName("Elite", 1, 2);
            }, "A ISelection's implementation with name 'Elite' was found, but seems the constructor args were invalid.");
        }

        [Test()]
        public void CreateSelectionByName_ValidName_SelectionCreated()
        {
            ISelection actual = SelectionService.CreateSelectionByName("Elite") as EliteSelection;
            ClassicAssert.IsNotNull(actual);

            actual = SelectionService.CreateSelectionByName("Rank") as RankSelection;
            ClassicAssert.IsNotNull(actual);

            actual = SelectionService.CreateSelectionByName("Roulette Wheel") as RouletteWheelSelection;
            ClassicAssert.IsNotNull(actual);

            actual = SelectionService.CreateSelectionByName("Tournament") as TournamentSelection;
            ClassicAssert.IsNotNull(actual);

            actual = SelectionService.CreateSelectionByName("Stochastic Universal Sampling") as StochasticUniversalSamplingSelection;
            ClassicAssert.IsNotNull(actual);
        }

        [Test()]
        public void GetSelectionTypeByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                SelectionService.GetSelectionTypeByName("Test");
            }, "There is no ISelection implementation with name 'Test'.");
        }

        [Test()]
        public void GetSelectionTypeByName_ValidName_SelectionTpe()
        {
            var actual = SelectionService.GetSelectionTypeByName("Elite");
            ClassicAssert.AreEqual(typeof(EliteSelection), actual);

            actual = SelectionService.GetSelectionTypeByName("Rank");
            ClassicAssert.AreEqual(typeof(RankSelection), actual);

            actual = SelectionService.GetSelectionTypeByName("Roulette Wheel");
            ClassicAssert.AreEqual(typeof(RouletteWheelSelection), actual);

            actual = SelectionService.GetSelectionTypeByName("Tournament");
            ClassicAssert.AreEqual(typeof(TournamentSelection), actual);

            actual = SelectionService.GetSelectionTypeByName("Stochastic Universal Sampling");
            ClassicAssert.AreEqual(typeof(StochasticUniversalSamplingSelection), actual);
        }
    }
}