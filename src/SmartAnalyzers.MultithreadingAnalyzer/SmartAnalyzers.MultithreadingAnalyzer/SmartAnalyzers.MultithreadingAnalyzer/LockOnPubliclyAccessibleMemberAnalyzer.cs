using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LockOnPubliclyAccessibleMemberAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MT1001";
        internal static readonly LocalizableString Title = "Lock on publicly accessible member";
        internal static readonly LocalizableString MessageFormat = "Locking on publicly accessible member can cause a deadlock'";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.RegisterSyntaxNodeAction(AnalyzeLockStatement, SyntaxKind.LockStatement);
        }

        private void AnalyzeLockStatement(SyntaxNodeAnalysisContext context)
        {
            var lockStatement = (LockStatementSyntax)context.Node;

            var expression = lockStatement?.Expression;
            if (expression == null)
            {
                return;
            }
            
            var expressionKind = expression.Kind();
            if (expressionKind == SyntaxKind.ThisExpression || expressionKind == SyntaxKind.SimpleMemberAccessExpression|| expressionKind == SyntaxKind.IdentifierName)
            {

                var symbolInfo = context.SemanticModel.GetSymbolInfo(expression);
                if (symbolInfo.Symbol is IPropertySymbol propertySymbol)
                {
                    if (new []{Accessibility.Internal, Accessibility.Public}.Contains(propertySymbol.DeclaredAccessibility))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, expression.GetLocation()));
                    }
                }
            }
        }
    }
}
