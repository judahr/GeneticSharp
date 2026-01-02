using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Infrastructure.Framework.UnitTests.Threading
{
    [TestFixture()]
    [Category("Infrastructure")]
    public class LinearTaskExecutorTest
    {
        [Test()]
        public void Start_Task_TaskRan()
        {
            var pipeline = "";
            var target = new LinearTaskExecutor();
            target.Add(() => pipeline += "1");
            target.Add(() => pipeline += "2");
            target.Add(() => pipeline += "3");

            ClassicAssert.IsTrue(target.Start());
            ClassicAssert.AreEqual("123", pipeline);
        }

        [Test()]
        public void Start_TakeMoreThanTimeout_False()
        {
            var pipeline = "";
            var target = new LinearTaskExecutor();
            target.Add(() => pipeline += "1");
            target.Add(() =>
            {
                pipeline += "2";
                Thread.Sleep(100);
            });
            target.Add(() => pipeline += "3");

            target.Timeout = TimeSpan.FromMilliseconds(100);
            ClassicAssert.IsFalse(target.Start());
            ClassicAssert.AreEqual("12", pipeline);
        }

        [Test()]
        public void Stop_ManyTasks_True()
        {
            var pipeline = "";
            var target = new LinearTaskExecutor();
            target.Add(() => pipeline += "1");
            target.Add(() =>
            {
                pipeline += "2";
                Thread.Sleep(1000);
            });
            target.Add(() => pipeline += "3");

            Parallel.Invoke(
                () => ClassicAssert.IsTrue(target.Start()),
                () =>
                {
                    Thread.Sleep(5);
                    target.Stop();
                });
        }
    }
}

