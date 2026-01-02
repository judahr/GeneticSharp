using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Infrastructure.Framework.UnitTests.Threading
{
    [TestFixture()]
    [Category("Infrastructure")]
    public class TaskExecutorBaseTest
    {
        [Test()]
        public void Add_Tasks_TasksAdded()
        {
            var target = new StubTaskExecutor();
            target.Add(() => { });
            target.Add(() => { });
            target.Add(() => { });

            ClassicAssert.AreEqual(3, target.GetTasks().Count);
        }

        [Test()]
        public void Clear_Tasks_TasksClean()
        {
            var target = new StubTaskExecutor();
            target.Add(() => { });
            target.Add(() => { });
            target.Add(() => { });

            target.Clear();
            ClassicAssert.AreEqual(0, target.GetTasks().Count);
        }

        [Test()]
        public void Start_NoArgs_StopRequestedFalse()
        {
            var target = new StubTaskExecutor();
            target.Start();
            ClassicAssert.IsFalse(target.GetStopRequested());
            target.Stop();
            target.Start();
            ClassicAssert.IsFalse(target.GetStopRequested());
        }

        [Test()]
        public void Stop_NoArgs_StopRequestedTrue()
        {
            var target = new StubTaskExecutor();
            target.Start();
            target.Stop();
            ClassicAssert.IsTrue(target.GetStopRequested());
        }
    }
}

