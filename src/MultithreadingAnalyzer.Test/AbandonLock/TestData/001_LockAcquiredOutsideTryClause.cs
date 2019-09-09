using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.AbandonLock.TestData
{
    class _001_LockAcquiredOutsideTryClause
    {
        private readonly object myLockObj = new object();

        public void DoSth1()
        {
            Monitor.Enter(myLockObj);
            try
            {

            }
            finally
            {
                Monitor.Exit(myLockObj);
            }
        }

        public void DoSth2()
        {
            var lockAcquired = Monitor.TryEnter(myLockObj);
            try
            {

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
            mySpinLock.Enter(ref lockAcquired);
            try
            {

            }
            finally
            {
                if(lockAcquired)
                    Monitor.Exit(myLockObj);
            }
        }

        public void DoSth4()
        {
            var lockAcquired = false;
            mySpinLock.TryEnter(ref lockAcquired);
            try
            {

            }
            finally
            {
                if(lockAcquired)
                    Monitor.Exit(myLockObj);
            }
        }

        private readonly Mutex myMutex = new Mutex();

        public void DoSth5()
        {
            myMutex.WaitOne();
            try
            {

            }
            finally
            {
               myMutex.ReleaseMutex();
            }
        }

        private readonly ReaderWriterLockSlim myReaderWriterLockSlim = new ReaderWriterLockSlim();

        public void DoSth6()
        {
            myReaderWriterLockSlim.EnterReadLock();
            try
            {

            }
            finally
            {
                myReaderWriterLockSlim.ExitReadLock();
            }
        }

        public void DoSth7()
        {
            myReaderWriterLockSlim.EnterWriteLock();
            try
            {

            }
            finally
            {
                myReaderWriterLockSlim.ExitWriteLock();
            }
        }

        public void DoSth8()
        {
            myReaderWriterLockSlim.EnterUpgradeableReadLock();
            try
            {

            }
            finally
            {
                myReaderWriterLockSlim.ExitUpgradeableReadLock();
            }
        }

        private readonly System.Threading.ReaderWriterLock myReaderWriterLock = new System.Threading.ReaderWriterLock();

        public void DoSth9()
        {
            myReaderWriterLock.AcquireReaderLock(1000);
            try
            {

            }
            finally
            {
                myReaderWriterLock.ReleaseReaderLock();
            }
        }

        public void DoSth10()
        {
            myReaderWriterLock.AcquireWriterLock(1000);
            try
            {

            }
            finally
            {
                myReaderWriterLock.ReleaseWriterLock();
            }
        }
    }
}
