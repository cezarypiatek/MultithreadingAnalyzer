using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection.TestData
{
    struct MyStruct
    {
        public int SampleProp { get; set; }
    }

    class _006_LockOnValueType
    {
        private MyStruct lockObj = new MyStruct();

        public void DoSth1()
        {
            Monitor.Enter(lockObj);
            Monitor.Exit(lockObj);
        }

        public void DoSth2()
        {
            var wasTaken = false;
            Monitor.TryEnter(lockObj, ref wasTaken);
            if (wasTaken)
            {
                Monitor.Exit(lockObj);
            }
        }
    }
}
