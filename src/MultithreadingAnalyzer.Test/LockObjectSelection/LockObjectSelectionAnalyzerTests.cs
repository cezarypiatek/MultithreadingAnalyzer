using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTestKit;

namespace SmartAnalyzers.MultithreadingAnalyzer.Test.LockObjectSelection
{
    public class LockObjectSelectionAnalyzerTests: AnalyzerTestFixture
    {
        [Test]
        public void do_not_lock_on_non_readonly_field()
        {
            HasDiagnosticAtLine(TestCases._001_LockOnNonReadonlyField, nameof(LockObjectSelectionAnalyzer.MT1003), 11);
        }

        [Test]
        public void do_not_monitor_enter_on_non_readonly_field()
        {
            HasDiagnosticAtLine(TestCases._001_LockOnNonReadonlyField, nameof(LockObjectSelectionAnalyzer.MT1003), 18);
        }

        [Test]
        public void do_not_monitor_try_enter_on_non_readonly_field()
        {
            HasDiagnosticAtLine(TestCases._001_LockOnNonReadonlyField, nameof(LockObjectSelectionAnalyzer.MT1003), 25);
        }

        [Test]
        public void do_not_lock_on_non_readonly_property()
        {
            HasDiagnosticAtLine(TestCases._002_LockOnNonReadonlyProperty, nameof(LockObjectSelectionAnalyzer.MT1003), 11);
        }

        [Test]
        public void do_not_monitor_enter_on_non_readonly_property()
        {
            HasDiagnosticAtLine(TestCases._002_LockOnNonReadonlyProperty, nameof(LockObjectSelectionAnalyzer.MT1003), 18);
        }

        [Test]
        public void do_not_monitor_try_enter_on_non_readonly_property()
        {
            HasDiagnosticAtLine(TestCases._002_LockOnNonReadonlyProperty, nameof(LockObjectSelectionAnalyzer.MT1003), 25);
        }

        [Test]
        public void do_not_lock_on_public_field()
        {
            HasDiagnosticAtLine(TestCases._003_LockOnPublicField, nameof(LockObjectSelectionAnalyzer.MT1000), 11);
        }

        [Test]
        public void do_not_monitor_enter_on_public_field()
        {
            HasDiagnosticAtLine(TestCases._003_LockOnPublicField, nameof(LockObjectSelectionAnalyzer.MT1000), 18);
        }

        [Test]
        public void do_not_monitor_try_enter_on_public_field()
        {
            HasDiagnosticAtLine(TestCases._003_LockOnPublicField, nameof(LockObjectSelectionAnalyzer.MT1000), 25);
        }

        [Test]
        public void do_not_lock_on_public_property()
        {
            HasDiagnosticAtLine(TestCases._004_LockOnPublicProperty, nameof(LockObjectSelectionAnalyzer.MT1000), 11);
        }

        [Test]
        public void do_not_monitor_enter_on_public_property()
        {
            HasDiagnosticAtLine(TestCases._004_LockOnPublicProperty, nameof(LockObjectSelectionAnalyzer.MT1000), 18);
        }

        [Test]
        public void do_not_monitor_try_enter_on_public_property()
        {
            HasDiagnosticAtLine(TestCases._004_LockOnPublicProperty, nameof(LockObjectSelectionAnalyzer.MT1000), 25);
        }


        [Test]
        public void do_not_monitor_enter_on_this_reference()
        {
            HasDiagnosticAtLine(TestCases._005_LockOnThisInstance, nameof(LockObjectSelectionAnalyzer.MT1001), 10);
        }

        [Test]
        public void do_not_monitor_try_enter_on_this_reference()
        {
            HasDiagnosticAtLine(TestCases._005_LockOnThisInstance, nameof(LockObjectSelectionAnalyzer.MT1001), 17);
        }

        [Test]
        public void do_not_monitor_enter_on_value_type()
        {
            HasDiagnosticAtLine(TestCases._006_LockOnValueType, nameof(LockObjectSelectionAnalyzer.MT1004), 16);
        }

        [Test]
        public void do_not_monitor_try_enter_on_value_type()
        {
            HasDiagnosticAtLine(TestCases._006_LockOnValueType, nameof(LockObjectSelectionAnalyzer.MT1004), 23);
        }

        [Test]
        public void do_not_lock_on_typeof()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 14);
        }

        [Test]
        public void do_not_lock_on_type_object()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 22);
        }

        [Test]
        public void do_not_monitor_on_type_object()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 30);
        }

        [Test]
        public void do_not_monitor_try_on_type_object()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 37);
        }

        [Test]
        public void do_not_lock_on_value_array()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 48);
        }

        [Test]
        public void do_not_monitor_on_value_array()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 56);
        }

        [Test]
        public void do_not_monitor_try_on_value_array()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 63);
        }

        [Test]
        public void do_not_lock_on_thread()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 74);
        }

        [Test]
        public void do_not_monitor_on_thread()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 82);
        }

        [Test]
        public void do_not_monitor_try_on_thread()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 89);
        }

        [Test]
        public void do_not_lock_on_string()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 100);
        }

        [Test]
        public void do_not_monitor_on_string()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 108);
        }

        [Test]
        public void do_not_monitor_try_on_string()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 115);
        }

        [Test]
        public void do_not_lock_on_parameter_info()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 128);
        }

        [Test]
        public void do_not_monitor_on_parameter_info()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 136);
        }

        [Test]
        public void do_not_monitor_try_on_parameter_info()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 143);
        }

        [Test]
        public void do_not_lock_on_marshal_by_ref()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 158);
        }

        [Test]
        public void do_not_monitor_on_marshal_by_ref()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 166);
        }

        [Test]
        public void do_not_monitor_try_on_marshal_by_ref()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 184);
        }

        [Test]
        public void do_not_lock_on_exception()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 158);
        }

        [Test]
        public void do_not_monitor_on_exception()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 192);
        }

        [Test]
        public void do_not_monitor_try_on_exception()
        {
            HasDiagnosticAtLine(TestCases._007_LockOnObjectWithWeakIdentity, nameof(LockObjectSelectionAnalyzer.MT1002), 199);
        }
    
        protected override string LanguageName => LanguageNames.CSharp;

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new LockObjectSelectionAnalyzer();
        }
    }
}
