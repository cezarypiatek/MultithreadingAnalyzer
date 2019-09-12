using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTestKit;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.CopiedSpinLock
{
    public class CopiedSpinLockAnalyzerTest: AnalyzerTestFixture
    {
        [Test]
        public void should_report_spinlock_pas_by_value_to_constructor() 
            => HasDiagnosticAtLine(TestCases._001_SpinLockPassByValue, nameof(CopiedSpinLockAnalyzer.MT1014), 7);

        [Test]
        public void should_report_spinlock_pas_by_value_to_method() 
            => HasDiagnosticAtLine(TestCases._001_SpinLockPassByValue, nameof(CopiedSpinLockAnalyzer.MT1014), 12);


        [Test]
        public void should_not_report_spinlock_pas_by_ref_to_constructor() 
            => NoDiagnosticAtLine(TestCases._001_SpinLockPassByValue, nameof(CopiedSpinLockAnalyzer.MT1014), 20);

        [Test]
        public void should_not_report_spinlock_pas_by_ref_to_method() 
            => NoDiagnosticAtLine(TestCases._001_SpinLockPassByValue, nameof(CopiedSpinLockAnalyzer.MT1014), 25);


        [Test]
        public void should_not_report_spinlock_pas_by_out_to_constructor() 
            => NoDiagnosticAtLine(TestCases._001_SpinLockPassByValue, nameof(CopiedSpinLockAnalyzer.MT1014), 33);

        [Test]
        public void should_not_report_spinlock_pas_by_out_to_method() 
            => NoDiagnosticAtLine(TestCases._001_SpinLockPassByValue, nameof(CopiedSpinLockAnalyzer.MT1014), 38);


        [Test]
        public void should_report_readonly_spinlock_pas_by_out_to_method() 
            => HasDiagnosticAtLine(TestCases._002_ReadonlySpinLock, nameof(CopiedSpinLockAnalyzer.MT1015), 7);

        [Test]
        public void should_not_report_non_readonly_spinlock_pas_by_out_to_method() 
            => NoDiagnosticAtLine(TestCases._002_ReadonlySpinLock, nameof(CopiedSpinLockAnalyzer.MT1015), 8);


        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new CopiedSpinLockAnalyzer();
    }
}