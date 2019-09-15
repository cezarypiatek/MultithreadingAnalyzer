using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.CopiedSpinLock.TestData
{
    class _001_SpinLockPassByValue
    {
        public _001_SpinLockPassByValue(SpinLock spinLock)
        {
            
        }

        public void SynchronizeWith(SpinLock spinLock)
        {

        }
    }

    class SampleSpinlockWrapper1
    {
        public SampleSpinlockWrapper1(ref SpinLock spinLock)
        {
            
        }

        public void SynchronizeWith(ref SpinLock spinLock)
        {

        }
    }

    class SampleSpinlockWrapper2
    {
        public SampleSpinlockWrapper2(out  SpinLock spinLock)
        {
            
        }

        public void SynchronizeWith(out SpinLock spinLock)
        {

        }
    }
}
