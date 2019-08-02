using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SmartAnalyzers.MultithreadingAnalyzer.Utils
{
    public static class ExpressionHelpers
    {
        public static bool IsInvocationOf(InvocationExpressionSyntax invocationExpression, string expectedMethodName)
        {
            if (invocationExpression.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                var text = memberAccess.ToFullString().Trim();
                return text == expectedMethodName || text.EndsWith($".{expectedMethodName}");
            }

            return false;
        }
    }
}
