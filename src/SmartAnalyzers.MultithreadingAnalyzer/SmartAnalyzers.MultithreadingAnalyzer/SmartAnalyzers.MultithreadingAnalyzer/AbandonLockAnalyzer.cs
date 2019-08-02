using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SmartAnalyzers.MultithreadingAnalyzer.Utils;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AbandonLockAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MT1003";
        internal static readonly LocalizableString Title = "Invoking Monitor.Exit without guarantee of execution";
        internal static readonly LocalizableString MessageFormat = "Monitor.Exit() should always be wrapped in finally to ensure execution";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            //TODO: Verify if Monitor.Enter is right before try or inside
            context.RegisterSyntaxNodeAction(AnalyzeMonitorMethodInvocation, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeMonitorMethodInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;
            if (ExpressionHelpers.IsInvocationOf(invocationExpression, "Monitor.Exit"))
            {
                TryReportViolation(context, invocationExpression.Parent, invocationExpression);
            }
        }

        private void TryReportViolation(SyntaxNodeAnalysisContext context, SyntaxNode memberAccessParent, SyntaxNode monitorExitExpression)
        {
            if (memberAccessParent is FinallyClauseSyntax)
            {
                return;
            }
            if (memberAccessParent is MethodDeclarationSyntax methodDeclaration && methodDeclaration.Parent is TypeDeclarationSyntax)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, monitorExitExpression.GetLocation()));     
            }
            else if(memberAccessParent.Parent !=null)
            {
                TryReportViolation(context, memberAccessParent.Parent, monitorExitExpression);
            }
        }
    }
}
