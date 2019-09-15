using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTestKit;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.AbandonLock
{
    public class AbandonLockAnalyzerTest : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new AbandonLockAnalyzer();
        protected override IReadOnlyCollection<MetadataReference> References => new[]
        {
            ReferenceSource.FromType<ReaderWriterLock>(),
        };

        [Test]
        public void should_report_monitor_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 11);

        [Test]
        public void should_report_monitor_try_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 24);

        [Test]
        public void should_report_spinlock_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 41);

        [Test]
        public void should_report_spinlock_try_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 56);

        [Test]
        public void should_report_mutext_wait_one_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 72);

        [Test]
        public void should_report_readerwriterlockslim_enter_read_lock_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 87);

        [Test]
        public void should_report_readerwriterlockslim_enter_write_lock_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 100);

        [Test]
        public void should_report_readerwriterlockslim_enter_upgradable_read_lock_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 113);


        [Test]
        public void should_report_readerwriterlock_enter_read_lock_outside_the_try_clause()
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 128);

        [Test]
        public void should_report_readerwriterlock_enter_write_lock_outside_the_try_clause()
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 141);

        [Test]
        public void should_not_report_lock_acquired_inside_the_try_clause() 
            => NoDiagnostic(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012));

        [Test]
        public void should_not_report_monitor_enter_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 13);

        [Test]
        public void should_not_report_monitor_try_enter_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 26);

        [Test]
        public void should_not_report_spinlock_enter_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 42);

        [Test]
        public void should_not_report_spinlock_try_enter_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 56);

        [Test]
        public void should_not_report_mutext_wait_one_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 71);

        [Test]
        public void should_not_report_readerwriterlockslim_enter_read_lock_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 85);

        [Test]
        public void should_not_report_readerwriterlockslim_enter_write_lock_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 97);

        [Test]
        public void should_not_report_readerwriterlockslim_enter_upgradable_read_lock_inside_the_try_clause() 
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 109);


        [Test]
        public void should_not_report_readerwriterlock_enter_read_lock_inside_the_try_clause()
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 123);

        [Test]
        public void should_not_report_readerwriterlock_enter_write_lock_inside_the_try_clause()
            => NoDiagnosticAtLine(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 135);

        [Test]
        public void should_not_report_lock_release_inside_the_finally_clause()
            => NoDiagnostic(TestCases._002_LockAcquiredInsideTryClause, nameof(AbandonLockAnalyzer.MT1013));

        [Test]
        public void should_report_monitor_exit_outside_the_finally_clause()
         => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 12);

        [Test]
        public void should_report_spinlock_exit_outside_the_finally_clause()
            => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 22);

        [Test]
        public void should_report_mutext_release_outside_the_finally_clause()
            => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 30);

        [Test]
        public void should_report_readerwriterlockslim_exit_read_lock_outside_the_finally_clause()
            => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 38);

        [Test]
        public void should_report_readerwriterlockslim_exit_write_lock_outside_the_finally_clause()
            => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 44);

        [Test]
        public void should_report_readerwriterlockslim_exit_upgradable_read_lock_outside_the_finally_clause()
            => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 50);


        [Test]
        public void should_report_readerwriterlock_exit_read_lock_outside_the_finally_clause()
            => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 58);

        [Test]
        public void should_report_readerwriterlock_exit_write_lock_outside_the_finally_clause()
            => HasDiagnosticAtLine(TestCases._003_LockReleasedOutsideFinallyClause, nameof(AbandonLockAnalyzer.MT1013), 64);
    }
}
