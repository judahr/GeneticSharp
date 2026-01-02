using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Domain.UnitTests.Randomizations
{
    [TestFixture()]
    [Category("Randomizations")]
    public class RandomizationProviderTest
    {
        [Test()]
        public void Current_Default_IsNotNull()
        {
            ClassicAssert.IsNotNull(RandomizationProvider.Current);
        }
    }
}

