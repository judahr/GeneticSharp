using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GeneticSharp.Infrastructure.Framework.UnitTests.Threading
{
    public class StubTaskExecutor : TaskExecutorBase
    {
        public ConcurrentBag<Action> GetTasks()
        {
            return Tasks;
        }

        public bool GetStopRequested()
        {
            return StopRequested;
        }
    }
}