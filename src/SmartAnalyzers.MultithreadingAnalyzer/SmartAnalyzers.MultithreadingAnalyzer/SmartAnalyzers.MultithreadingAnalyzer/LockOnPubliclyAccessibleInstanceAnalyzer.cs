using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LockOnPubliclyAccessibleInstanceAnalyzer : BasicLockAnalyzer
    {
        public const string DiagnosticId = "MT1000";
        internal static readonly LocalizableString Title = "Lock on publicly accessible instance";
        internal static readonly LocalizableString MessageFormat = "Locking on publicly accessible instance can cause a deadlock";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);


        protected override void TryToReportViolation(SyntaxNodeAnalysisContext context, ExpressionSyntax expression)
        {
            if (expression == null)
            {
                return;
            }

            var expressionKind = expression.Kind();
            if (expressionKind == SyntaxKind.ThisExpression || expressionKind == SyntaxKind.TypeOfExpression)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, expression.GetLocation()));
            }
        }
    }
}
