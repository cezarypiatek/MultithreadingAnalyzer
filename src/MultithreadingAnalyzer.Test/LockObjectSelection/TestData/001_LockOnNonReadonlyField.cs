using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection.TestData
{
    class _001_LockOnNonReadonlyField
    {
        private object lockobj = new object();

        public void DoSth1()
        {
            lock (lockobj)
            {
            }
        }

        public void DoSth2()
        {
            Monitor.Enter(lockobj);
            Monitor.Exit(lockobj);
        }

        public void DoSth3()
        {
            var wasTaken = false;
            Monitor.TryEnter(lockobj, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(lockobj);
            }
        }
    }
}
