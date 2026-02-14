using System;
using GeneticSharp.Domain.Mutations;
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

            ClassicAssert.LessOrEqual(10, actual.Count);
            int i = 0;
            ClassicAssert.AreEqual(typeof(DeletionMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(DisplacementMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(FlipBitMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(GeneMutation), actual[i]); i++;

            ClassicAssert.AreEqual(typeof(InsertionMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(PartialShuffleMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(RandomMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(ReverseSequenceMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(TworsMutation), actual[i]); i++;
            ClassicAssert.AreEqual(typeof(UniformMutation), actual[i]); i++;
        }

        [Test()]
        public void GetMutationNames_NoArgs_AllAvailableMutationsNames()
        {
            var actual = MutationService.GetMutationNames();

            ClassicAssert.LessOrEqual(10, actual.Count);
            int i = 0;
            ClassicAssert.AreEqual("Deletion", actual[i]); i++;
            ClassicAssert.AreEqual("Displacement", actual[i]);i++;
            ClassicAssert.AreEqual("Flip Bit", actual[i]); i++;
            ClassicAssert.AreEqual("GeneMutation", actual[i]); i++;
            ClassicAssert.AreEqual("Insertion", actual[i]); i++;
            ClassicAssert.AreEqual("Partial Shuffle (PSM)", actual[i]); i++;
            ClassicAssert.AreEqual("RandomMutation", actual[i]); i++; 
            ClassicAssert.AreEqual("Reverse Sequence (RSM)", actual[i]); i++;            
            ClassicAssert.AreEqual("Twors", actual[i]); i++;
            ClassicAssert.AreEqual("Uniform", actual[i]); i++;
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