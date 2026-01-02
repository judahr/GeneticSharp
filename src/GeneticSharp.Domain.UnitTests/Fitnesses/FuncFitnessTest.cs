using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Fitnesses
{
    [TestFixture]
    public class FuncFitnessTest
    {
        [Test]
        public void Evaluate_Func_CallFunc()
        {
            var target = new FuncFitness((c) =>
            {
                return c.Fitness.Value + 1;
            });

            ClassicAssert.AreEqual(3, target.Evaluate(new ChromosomeStub(2d)));
        }
    }
}

