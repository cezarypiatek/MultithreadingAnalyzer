using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LockedObjectTypeAnalyzer : BasicLockAnalyzer
    {
        public const string DiagnosticId = "MT1008";
        internal static readonly LocalizableString Title = "Lock on invalid object type";
        internal static readonly LocalizableString MessageFormat = "{0}";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        private const string ReasonForValueType = "Locking on value types prohibits any synchronization because they are boxed";
        private const string ReasonForStringType = "Locking on System.String can cause deadlock because of string interning";
        private const string ReasonForSystemTypeType = "Locking on System.Type can cause deadlock because the instances are shared across the application";

        protected override void TryToReportViolation(SyntaxNodeAnalysisContext context, ExpressionSyntax expression)
        {
            if (expression == null)
            {
                return;
            }

            var symbolInfo = context.SemanticModel.GetTypeInfo(expression);
            if (symbolInfo.Type != null)
            {
                if (symbolInfo.Type.IsValueType)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, expression.GetLocation(), ReasonForValueType));
                }
                else if (symbolInfo.Type.ToString() == "string")
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, expression.GetLocation(), ReasonForStringType));
                }
                else if(CanBeAssignedTo(symbolInfo.Type, "System.Type"))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, expression.GetLocation(), ReasonForSystemTypeType));
                }
            }
        }

        private bool CanBeAssignedTo(ITypeSymbol symbolInfoType, string baseType)
        {
            if (symbolInfoType == null)
            {
                return false;
            }

            if (symbolInfoType.ToString() == baseType)
            {
                return true;
            }
            return CanBeAssignedTo(symbolInfoType.BaseType, baseType);
        }
    }
}