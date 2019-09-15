using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTestKit;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.DeprecatedReaderWriterLock
{
    public class DeprecatedReaderWriterLockAnalyzerTest : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new DeprecatedReaderWriterLockAnalyzer();

        [Test]
        public void should_report_readerwriterlock_property() => 
            HasDiagnosticAtLine(TestCases._001_ReaderWriterLock, nameof(DeprecatedReaderWriterLockAnalyzer.MT1016), 7);

        [Test]
        public void should_report_readerwriterlock_field() => 
            HasDiagnosticAtLine(TestCases._001_ReaderWriterLock, nameof(DeprecatedReaderWriterLockAnalyzer.MT1016), 9);
    }
}