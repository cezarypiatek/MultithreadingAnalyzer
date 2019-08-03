using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LockOnPubliclyAccessibleMemberAnalyzer : BasicLockAnalyzer
    {
        public const string DiagnosticId = "MT1001";
        internal static readonly LocalizableString Title = "Lock on publicly accessible member";
        internal static readonly LocalizableString MessageFormat = "Locking on publicly accessible member can cause a deadlock'";
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
            if (expressionKind == SyntaxKind.SimpleMemberAccessExpression || expressionKind == SyntaxKind.IdentifierName)
            {

                var symbolInfo = context.SemanticModel.GetSymbolInfo(expression);
                if (symbolInfo.Symbol is IPropertySymbol || symbolInfo.Symbol is IFieldSymbol)
                {
                    if (symbolInfo.Symbol.DeclaredAccessibility != Accessibility.Private)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, expression.GetLocation()));
                    }
                }
            }
        }
    }
}
