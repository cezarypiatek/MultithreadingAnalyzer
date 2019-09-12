using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.AbandonLock.TestData
{
    class _002_LockAcquiredInsideTryClause
    {
        private readonly object myLockObj = new object();

        public void DoSth1()
        {
            try
            {
                Monitor.Enter(myLockObj);
            }
            finally
            {
                Monitor.Exit(myLockObj);
            }
        }

        public void DoSth2()
        {
            var lockAcquired = false;
            try
            {
                lockAcquired = Monitor.TryEnter(myLockObj);
            }
            finally
            {
                if(lockAcquired)
                    Monitor.Exit(myLockObj);
            }
        }

        private SpinLock mySpinLock = new SpinLock();

        public void DoSth3()
        {
            var lockAcquired = false;
            try
            {
                mySpinLock.Enter(ref lockAcquired);
            }
            finally
            {
                if(lockAcquired)
                    mySpinLock.Exit();
            }
        }

        public void DoSth4()
        {
            var lockAcquired = false;
            try
            {
                mySpinLock.TryEnter(ref lockAcquired);
            }
            finally
            {
                if(lockAcquired)
                    mySpinLock.Exit();
            }
        }

        private readonly Mutex myMutex = new Mutex();

        public void DoSth5()
        {
            try
            {
                myMutex.WaitOne();
            }
            finally
            {
               myMutex.ReleaseMutex();
            }
        }

        private readonly ReaderWriterLockSlim myReaderWriterLockSlim = new ReaderWriterLockSlim();

        public void DoSth6()
        {
            try
            {
                myReaderWriterLockSlim.EnterReadLock();
            }
            finally
            {
                myReaderWriterLockSlim.ExitReadLock();
            }
        }

        public void DoSth7()
        {
            try
            {
                myReaderWriterLockSlim.EnterWriteLock();
            }
            finally
            {
                myReaderWriterLockSlim.ExitWriteLock();
            }
        }

        public void DoSth8()
        {
            try
            {
                myReaderWriterLockSlim.EnterUpgradeableReadLock();
            }
            finally
            {
                myReaderWriterLockSlim.ExitUpgradeableReadLock();
            }
        }

        private readonly System.Threading.ReaderWriterLock myReaderWriterLock = new System.Threading.ReaderWriterLock();

        public void DoSth9()
        {
            try
            {
                myReaderWriterLock.AcquireReaderLock(1000);
            }
            finally
            {
                myReaderWriterLock.ReleaseReaderLock();
            }
        }

        public void DoSth10()
        {
            try
            {
                myReaderWriterLock.AcquireWriterLock(1000);
            }
            finally
            {
                myReaderWriterLock.ReleaseWriterLock();
            }
        }
    }
}
