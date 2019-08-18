using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynNUnitLight;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection
{
    public class LockObjectSelectionAnalyzerTests: AnalyzerTestFixture
    {
        [Test]
        public void do_not_lock_on_non_readonly_field()
        {
            HasDiagnostic(TestCases._001_LockOnNonReadonlyField, 11, nameof(LockObjectSelectionAnalyzer.MT1003));
        }

        [Test]
        public void do_not_monitor_enter_on_non_readonly_field()
        {
            HasDiagnostic(TestCases._001_LockOnNonReadonlyField, 18, nameof(LockObjectSelectionAnalyzer.MT1003));
        }

        [Test]
        public void do_not_monitor_try_enter_on_non_readonly_field()
        {
            HasDiagnostic(TestCases._001_LockOnNonReadonlyField, 25, nameof(LockObjectSelectionAnalyzer.MT1003));
        }

        [Test]
        public void do_not_lock_on_non_readonly_property()
        {
            HasDiagnostic(TestCases._002_LockOnNonReadonlyProperty, 11, nameof(LockObjectSelectionAnalyzer.MT1003));
        }

        [Test]
        public void do_not_monitor_enter_on_non_readonly_property()
        {
            HasDiagnostic(TestCases._002_LockOnNonReadonlyProperty, 18, nameof(LockObjectSelectionAnalyzer.MT1003));
        }

        [Test]
        public void do_not_monitor_try_enter_on_non_readonly_property()
        {
            HasDiagnostic(TestCases._002_LockOnNonReadonlyProperty, 25, nameof(LockObjectSelectionAnalyzer.MT1003));
        }

        [Test]
        public void do_not_lock_on_public_field()
        {
            HasDiagnostic(TestCases._003_LockOnPublicField, 11, nameof(LockObjectSelectionAnalyzer.MT1000));
        }

        [Test]
        public void do_not_monitor_enter_on_public_field()
        {
            HasDiagnostic(TestCases._003_LockOnPublicField, 18, nameof(LockObjectSelectionAnalyzer.MT1000));
        }

        [Test]
        public void do_not_monitor_try_enter_on_public_field()
        {
            HasDiagnostic(TestCases._003_LockOnPublicField, 25, nameof(LockObjectSelectionAnalyzer.MT1000));
        }

        [Test]
        public void do_not_lock_on_public_property()
        {
            HasDiagnostic(TestCases._004_LockOnPublicProperty, 11, nameof(LockObjectSelectionAnalyzer.MT1000));
        }

        [Test]
        public void do_not_monitor_enter_on_public_property()
        {
            HasDiagnostic(TestCases._004_LockOnPublicProperty, 18, nameof(LockObjectSelectionAnalyzer.MT1000));
        }

        [Test]
        public void do_not_monitor_try_enter_on_public_property()
        {
            HasDiagnostic(TestCases._004_LockOnPublicProperty, 25, nameof(LockObjectSelectionAnalyzer.MT1000));
        }


        [Test]
        public void do_not_monitor_enter_on_this_reference()
        {
            HasDiagnostic(TestCases._005_LockOnThisInstance, 10, nameof(LockObjectSelectionAnalyzer.MT1001));
        }

        [Test]
        public void do_not_monitor_try_enter_on_this_reference()
        {
            HasDiagnostic(TestCases._005_LockOnThisInstance, 17, nameof(LockObjectSelectionAnalyzer.MT1001));
        }

        [Test]
        public void do_not_monitor_enter_on_value_type()
        {
            HasDiagnostic(TestCases._006_LockOnValueType, 16, nameof(LockObjectSelectionAnalyzer.MT1004));
        }

        [Test]
        public void do_not_monitor_try_enter_on_value_type()
        {
            HasDiagnostic(TestCases._006_LockOnValueType, 23, nameof(LockObjectSelectionAnalyzer.MT1004));
        }

        [Test]
        public void do_not_lock_on_typeof()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 14, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_lock_on_type_object()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 22, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_on_type_object()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 30, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_try_on_type_object()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 37, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_lock_on_value_array()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 48, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_on_value_array()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 56, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_try_on_value_array()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 63, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_lock_on_thread()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 74, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_on_thread()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 82, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_try_on_thread()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 89, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_lock_on_string()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 100, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_on_string()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 108, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_try_on_string()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 115, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_lock_on_parameter_info()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 128, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_on_parameter_info()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 136, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_try_on_parameter_info()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 143, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_lock_on_marshal_by_ref()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 158, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_on_marshal_by_ref()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 166, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_try_on_marshal_by_ref()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 184, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_lock_on_exception()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 158, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_on_exception()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 192, nameof(LockObjectSelectionAnalyzer.MT1002));
        }

        [Test]
        public void do_not_monitor_try_on_exception()
        {
            HasDiagnostic(TestCases._007_LockOnObjectWithWeakIdentity, 199, nameof(LockObjectSelectionAnalyzer.MT1002));
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
            return new LockObjectSelectionAnalyzer();
        }
    }
}
