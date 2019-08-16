using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection.TestData
{
    class _002_LockOnNonReadonlyProperty
    {
        private object LockObj { get; set; } = new object();

        public void DoSth1()
        {
            lock (LockObj)
            {
            }
        }

        public void DoSth2()
        {
            Monitor.Enter(LockObj);
            Monitor.Exit(LockObj);
        }

        public void DoSth3()
        {
            var wasTaken = false;
            Monitor.TryEnter(LockObj, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(LockObj);
            }
        }
    }
}
