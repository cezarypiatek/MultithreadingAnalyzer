using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTestKit;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.AbandonLock
{
    public class AbandonLockAnalyzerTest : AnalyzerTestFixture
    {
        [Test]
        public void should_be_able_report_monitor_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 11); 

        [Test]
        public void should_be_able_report_monitor_try_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 24);

        [Test]
        public void should_be_able_report_spinlock_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 41);

        [Test]
        public void should_be_able_report_spinlock_try_enter_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 56);
       
        [Test]
        public void should_be_able_report_mutext_wait_one_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 72);

        [Test]
        public void should_be_able_report_readerwriterlockslim_enter_read_lock_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 87);

        [Test]
        public void should_be_able_report_readerwriterlockslim_enter_write_lock_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 100);

        [Test]
        public void should_be_able_report_readerwriterlockslim_enter_upgradable_read_lock_outside_the_try_clause() 
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 113);


        [Test]
        public void should_be_able_report_readerwriterlock_enter_read_lock_outside_the_try_clause()
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 128);

        [Test]
        public void should_be_able_report_readerwriterlock_enter_write_lock_outside_the_try_clause()
            => HasDiagnosticAtLine(TestCases._001_LockAcquiredOutsideTryClause, nameof(AbandonLockAnalyzer.MT1012), 141);


        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new AbandonLockAnalyzer();

        protected override ImmutableList<MetadataReference> References => new[]
        {
            RoslynTestKit.References.FromType<ReaderWriterLock>()
        }.ToImmutableList();
    }
}
