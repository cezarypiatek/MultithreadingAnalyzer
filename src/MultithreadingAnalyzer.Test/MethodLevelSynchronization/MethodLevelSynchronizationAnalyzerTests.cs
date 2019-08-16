using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynNUnitLight;
using static SmartAnalyzers.MultithreadingAnalyzer.Test.MethodLevelSynchronization.TestCases;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.MethodLevelSynchronization
{
    public class MethodLevelSynchronizationAnalyzerTests : AnalyzerTestFixture
    {
        [Test]
        public void do_not_use_method_level_synchronization()
        {
            HasDiagnostic(_001_MethodLevelSyncrhonization, 9, MethodLevelSynchronizationAnalyzer.DiagnosticId);
        }

        [Test]
        public void do_not_use_method_level_synchronization_multiple_flags()
        {
            HasDiagnostic(_001_MethodLevelSyncrhonization, 14, MethodLevelSynchronizationAnalyzer.DiagnosticId);
        }

        protected void HasDiagnostic(string code, int line, string diagnosticId)
        {
            var zeroBaseLine = line - 1;
            var document = TestHelpers.GetDocument(code, this.LanguageName, (ImmutableList<MetadataReference>) null);
            var diagnostics = this.GetDiagnostics(document);
            var diagnostic = diagnostics.FirstOrDefault(x => 
                x.Id == diagnosticId && 
                x.Location.GetLineSpan().StartLinePosition.Line <= zeroBaseLine &&
                x.Location.GetLineSpan().EndLinePosition.Line >= zeroBaseLine &&
                x.Location.IsInSource);
            Assert.IsNotNull(diagnostic, $"There is no diagnostic {diagnosticId} at line {line}");
        }

        private ImmutableArray<Diagnostic> GetDiagnostics(Document document)
        {
            ImmutableArray<DiagnosticAnalyzer> immutableArray = ImmutableArray.Create<DiagnosticAnalyzer>(this.CreateAnalyzer());
            Compilation result1 = document.Project.GetCompilationAsync(CancellationToken.None).Result;
            CompilationWithAnalyzers compilationWithAnalyzers = DiagnosticAnalyzerExtensions.WithAnalyzers(result1, immutableArray, (AnalyzerOptions)null, CancellationToken.None);
            result1.GetDiagnostics(CancellationToken.None);
            SyntaxTree result2 = document.GetSyntaxTreeAsync(CancellationToken.None).Result;
            ImmutableArray<Diagnostic>.Builder builder = ImmutableArray.CreateBuilder<Diagnostic>();
            foreach (Diagnostic diagnostic in compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result)
            {
                Location location = diagnostic.Location;
                if (location.IsInSource && location.SourceTree == result2)
                    builder.Add(diagnostic);
            }
            return builder.ToImmutable();
        }
        protected override string LanguageName => LanguageNames.CSharp;

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new MethodLevelSynchronizationAnalyzer();
        }
    }
}
