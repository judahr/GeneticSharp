using System;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Mutations
{
    [TestFixture()]
    [Category("Mutations")]
    public class MutationServiceTest
    {
        [Test()]
        public void GetMutationTypes_NoArgs_AllAvailableMutations()
        {
            var actual = MutationService.GetMutationTypes();

            ClassicAssert.AreEqual(7, actual.Count);
            ClassicAssert.AreEqual(typeof(DisplacementMutation), actual[0]);
            ClassicAssert.AreEqual(typeof(FlipBitMutation), actual[1]);
            ClassicAssert.AreEqual(typeof(InsertionMutation), actual[2]);
            ClassicAssert.AreEqual(typeof(PartialShuffleMutation), actual[3]);
            ClassicAssert.AreEqual(typeof(ReverseSequenceMutation), actual[4]);
            ClassicAssert.AreEqual(typeof(TworsMutation), actual[5]);
            ClassicAssert.AreEqual(typeof(UniformMutation), actual[6]);
        }

        [Test()]
        public void GetMutationNames_NoArgs_AllAvailableMutationsNames()
        {
            var actual = MutationService.GetMutationNames();

            ClassicAssert.AreEqual(7, actual.Count);
            ClassicAssert.AreEqual("Displacement", actual[0]);
            ClassicAssert.AreEqual("Flip Bit", actual[1]);
            ClassicAssert.AreEqual("Insertion", actual[2]);
            ClassicAssert.AreEqual("Partial Shuffle (PSM)", actual[3]);
            ClassicAssert.AreEqual("Reverse Sequence (RSM)", actual[4]);
            ClassicAssert.AreEqual("Twors", actual[5]);
            ClassicAssert.AreEqual("Uniform", actual[6]);
        }

        [Test()]
        public void CreateMutationByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                MutationService.CreateMutationByName("Test");
            }, "There is no IMutation implementation with name 'Test'.");
        }

        [Test()]
        public void CreateMutationByName_ValidNameButInvalidConstructorArgs_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                MutationService.CreateMutationByName("Uniform", 1f);
            }, "A IMutation's implementation with name 'Uniform' was found, but seems the constructor args were invalid.");
        }

        [Test()]
        public void CreateMutationByName_ValidName_MutationCreated()
        {
            IMutation actual = MutationService.CreateMutationByName("Reverse Sequence (RSM)") as ReverseSequenceMutation;
            ClassicAssert.IsNotNull(actual);

            actual = MutationService.CreateMutationByName("Twors") as TworsMutation;
            ClassicAssert.IsNotNull(actual);

            actual = MutationService.CreateMutationByName("Uniform", true) as UniformMutation;
            ClassicAssert.IsNotNull(actual);
        }

        [Test()]
        public void GetMutationTypeByName_InvalidName_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                MutationService.GetMutationTypeByName("Test");
            }, "There is no IMutation implementation with name 'Test'.");
        }

        [Test()]
        public void GetMutationTypeByName_ValidName_CrossoverTpe()
        {
            var actual = MutationService.GetMutationTypeByName("Reverse Sequence (RSM)");
            ClassicAssert.AreEqual(typeof(ReverseSequenceMutation), actual);

            actual = MutationService.GetMutationTypeByName("Twors");
            ClassicAssert.AreEqual(typeof(TworsMutation), actual);

            actual = MutationService.GetMutationTypeByName("Uniform");
            ClassicAssert.AreEqual(typeof(UniformMutation), actual);
        }

        [Test()]
        public void Shuffle_Source_Shuffled()
        {
            var rnd = Substitute.For<IRandomization>();
            rnd.GetInt(0, 5).Returns(4);
            rnd.GetInt(0, 4).Returns(2);
            rnd.GetInt(0, 3).Returns(3);
            rnd.GetInt(0, 2).Returns(0);
            rnd.GetInt(0, 1).Returns(1);

            var actual = new int[] { 1, 2, 3, 4, 5 }.Shuffle(rnd);
            CollectionAssert.AreEqual(new int[] { 5, 3, 4, 1, 2 }, actual);
        }

        [Test()]
        public void LeftShift_ValueToShift_Shifted()
        {
             var actual = new int[] { 1, 2, 3, 4, 5 }.LeftShift(2);
            CollectionAssert.AreEqual(new int[] { 3, 4, 5, 1, 2 }, actual);
        }

        [Test()]
        public void RightShift_ValueToShift_Shifted()
        {
            var actual = new int[] { 1, 2, 3, 4, 5 }.RightShift(2);
            CollectionAssert.AreEqual(new int[] { 4, 5, 1, 2, 3 }, actual);
        }
    }
}