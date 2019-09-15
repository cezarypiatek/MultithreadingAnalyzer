using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.CopiedSpinLock.TestData
{
    class _002_ReadonlySpinLock
    {
        private readonly SpinLock InvalidSpinLock = new SpinLock();
        private SpinLock ValidSpinLock = new SpinLock();
    }
}
