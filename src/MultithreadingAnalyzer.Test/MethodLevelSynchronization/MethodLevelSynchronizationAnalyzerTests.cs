using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTestKit;
using static SmartAnalyzers.MultithreadingAnalyzer.Test.MethodLevelSynchronization.TestCases;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.MethodLevelSynchronization
{
    public class MethodLevelSynchronizationAnalyzerTests : AnalyzerTestFixture
    {
        [Test]
        public void do_not_use_method_level_synchronization()
        {
            HasDiagnosticAtLine(_001_MethodLevelSyncrhonization,  MethodLevelSynchronizationAnalyzer.DiagnosticId,9 );
        }

        [Test]
        public void do_not_use_method_level_synchronization_multiple_flags()
        {
            HasDiagnosticAtLine(_001_MethodLevelSyncrhonization, MethodLevelSynchronizationAnalyzer.DiagnosticId, 14);
        }
        
        protected override string LanguageName => LanguageNames.CSharp;

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new MethodLevelSynchronizationAnalyzer();
        }
    }
}
