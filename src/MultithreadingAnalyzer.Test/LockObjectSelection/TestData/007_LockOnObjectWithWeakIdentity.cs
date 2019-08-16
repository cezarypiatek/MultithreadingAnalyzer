using System;
using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection.TestData
{
    class _007_LockOnObjectWithWeakIdentity
    {
        private readonly Type lockType = typeof(_007_LockOnObjectWithWeakIdentity);

        public void DoSth1()
        {
            lock (typeof(_007_LockOnObjectWithWeakIdentity))
            {

            }
        }

        public void DoSth2()
        {
            lock (lockType)
            {

            }
        }

        public void DoSth3()
        {
            Monitor.Enter(lockType);
            Monitor.Exit(lockType);
        }

        public void DoSth4()
        {
            var wasTaken = false;
            Monitor.TryEnter(lockType, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(lockType);
            }
        }

        private readonly int[] lockValueArray = new int[10];

        public void DoSth5()
        {
            lock (lockValueArray)
            {

            }
        }

        public void DoSth6()
        {
            Monitor.Enter(lockValueArray);
            Monitor.Exit(lockValueArray);
        }

        public void DoSth7()
        {
            var wasTaken = false;
            Monitor.TryEnter(lockValueArray, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(lockValueArray);
            }
        }

        private readonly Thread lockThread = new Thread(() => {});

        public void DoSth8()
        {
            lock (lockThread)
            {

            }
        }

        public void DoSth9()
        {
            Monitor.Enter(lockThread);
            Monitor.Exit(lockThread);
        }

        public void DoSth10()
        {
            var wasTaken = false;
            Monitor.TryEnter(lockThread, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(lockThread);
            }
        }

        private readonly string lockTString = "sample";

        public void DoSth11()
        {
            lock (lockTString)
            {

            }
        }

        public void DoSth12()
        {
            Monitor.Enter(lockTString);
            Monitor.Exit(lockTString);
        }

        public void DoSth13()
        {
            var wasTaken = false;
            Monitor.TryEnter(lockTString, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(lockTString);
            }
        }
    }
}
