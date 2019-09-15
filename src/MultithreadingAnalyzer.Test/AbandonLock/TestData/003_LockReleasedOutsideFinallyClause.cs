using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.AbandonLock.TestData
{
    class _003_LockReleasedOutsideFinallyClause
    {
        private readonly object myLockObj = new object();

        public void DoSth1()
        {
            Monitor.Enter(myLockObj);
            Monitor.Exit(myLockObj);
        }

        private SpinLock mySpinLock = new SpinLock();

        public void DoSth4()
        {
            var lockAcquired = false;
            mySpinLock.TryEnter(ref lockAcquired);
            if (lockAcquired)
                mySpinLock.Exit();
        }

        private readonly Mutex myMutex = new Mutex();

        public void DoSth5()
        {
            myMutex.WaitOne();
            myMutex.ReleaseMutex();
        }

        private readonly ReaderWriterLockSlim myReaderWriterLockSlim = new ReaderWriterLockSlim();

        public void DoSth6()
        {
            myReaderWriterLockSlim.EnterReadLock();
            myReaderWriterLockSlim.ExitReadLock();
        }

        public void DoSth7()
        {
            myReaderWriterLockSlim.EnterWriteLock();
            myReaderWriterLockSlim.ExitWriteLock();
        }

        public void DoSth8()
        {
            myReaderWriterLockSlim.EnterUpgradeableReadLock();
            myReaderWriterLockSlim.ExitUpgradeableReadLock();
        }

        private readonly System.Threading.ReaderWriterLock myReaderWriterLock = new System.Threading.ReaderWriterLock();

        public void DoSth9()
        {
            myReaderWriterLock.AcquireReaderLock(1000);
            myReaderWriterLock.ReleaseReaderLock();
        }

        public void DoSth10()
        {
            myReaderWriterLock.AcquireWriterLock(1000);
            myReaderWriterLock.ReleaseWriterLock();
        }
    }
}
