using System.Runtime.CompilerServices;
using System.Threading;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection.TestData
{
    class _001_MethodLevelSynchronization
    {

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DoSth3()
        {
        }

        [MethodImpl( MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
        public void DoSth4()
        {
        }
    }
}
