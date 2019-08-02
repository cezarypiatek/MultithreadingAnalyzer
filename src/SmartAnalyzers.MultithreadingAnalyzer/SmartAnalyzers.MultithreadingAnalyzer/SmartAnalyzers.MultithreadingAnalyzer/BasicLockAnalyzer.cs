using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SmartAnalyzers.MultithreadingAnalyzer.Utils;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    public abstract class BasicLockAnalyzer : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.RegisterSyntaxNodeAction(AnalyzeLockStatement, SyntaxKind.LockStatement);
            context.RegisterSyntaxNodeAction(AnalyzeMonitorEnterInvocation, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeLockStatement(SyntaxNodeAnalysisContext context)
        {
            var lockStatement = (LockStatementSyntax)context.Node;

            TryToReportViolation(context, lockStatement?.Expression);
        }

        private void AnalyzeMonitorEnterInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;
            if (ExpressionHelpers.IsInvocationOf(invocationExpression, "Monitor.Enter") ||
                ExpressionHelpers.IsInvocationOf(invocationExpression, "Monitor.TryEnter"))
            {
                var lockParameterExpression = invocationExpression.ArgumentList?.Arguments.FirstOrDefault()?.Expression;
                TryToReportViolation(context, lockParameterExpression);
            }
        }

        protected abstract void TryToReportViolation(SyntaxNodeAnalysisContext context, ExpressionSyntax expression);
    }
}