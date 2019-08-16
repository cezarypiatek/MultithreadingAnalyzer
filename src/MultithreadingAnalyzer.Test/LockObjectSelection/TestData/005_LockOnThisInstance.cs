using System.Runtime.CompilerServices;
using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection.TestData
{
    class _005_LockOnThisInstance
    {
        public void DoSth1()
        {
            Monitor.Enter(this);
            Monitor.Exit(this);
        }

        public void DoSth2()
        {
            var wasTaken = false;
            Monitor.TryEnter(this, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(this);
            }
        }
    }
}
