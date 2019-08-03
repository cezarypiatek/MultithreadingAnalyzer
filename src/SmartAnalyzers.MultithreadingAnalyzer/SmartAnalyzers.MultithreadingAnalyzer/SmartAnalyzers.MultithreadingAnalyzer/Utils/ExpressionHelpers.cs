using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer.Utils
{
    internal static class ExpressionHelpers
    {
        public static bool IsInvocationOf(SyntaxNodeAnalysisContext context, params MethodDescriptor[] candidates)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;
            if (invocationExpression.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                var text = memberAccess.ToFullString().Trim();
                ISymbol symbol = null;
                for (int i = 0; i < candidates.Length; i++)
                {
                    if (text == candidates[i].Name || text.EndsWith($".{candidates[i].Name}"))
                    {
                        if (symbol == null)
                        {
                            symbol = context.SemanticModel.GetSymbolInfo(memberAccess).Symbol;
                            if (symbol == null)
                            {
                                return false;
                            }
                        }

                        if (symbol.Name == candidates[i].Name && symbol.ContainingType.ToString() == candidates[i].TypeFullName)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
