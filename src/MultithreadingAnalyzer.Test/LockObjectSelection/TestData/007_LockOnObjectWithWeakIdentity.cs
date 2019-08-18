using System;
using System.Linq;
using System.Reflection;
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

        
        private readonly ParameterInfo parameterInfoLock = typeof(_007_LockOnObjectWithWeakIdentity)
            .GetMethod(nameof(DoSth14)).GetParameters().First();

        public void DoSth14(int param)
        {
            lock (parameterInfoLock)
            {

            }
        }

        public void DoSth15()
        {
            Monitor.Enter(parameterInfoLock);
            Monitor.Exit(parameterInfoLock);
        }

        public void DoSth16()
        {
            var wasTaken = false;
            Monitor.TryEnter(parameterInfoLock, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(parameterInfoLock);
            }
        }

        public class Worker : MarshalByRefObject
        {
        }

        private readonly Worker marshalByRefLock = new Worker();

        public void DoSth17()
        {
            lock (marshalByRefLock)
            {

            }
        }

        public void DoSth18()
        {
            Monitor.Enter(marshalByRefLock);
            Monitor.Exit(marshalByRefLock);
        }

        public void DoSth19()
        {
            var wasTaken = false;
            Monitor.TryEnter(marshalByRefLock, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(marshalByRefLock);
            }
        }

        private readonly Exception exceptionLock = new OutOfMemoryException();

        public void DoSth20()
        {
            lock (exceptionLock)
            {

            }
        }

        public void DoSth21()
        {
            Monitor.Enter(exceptionLock);
            Monitor.Exit(exceptionLock);
        }

        public void DoSth22()
        {
            var wasTaken = false;
            Monitor.TryEnter(exceptionLock, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(exceptionLock);
            }
        }
    }
}
