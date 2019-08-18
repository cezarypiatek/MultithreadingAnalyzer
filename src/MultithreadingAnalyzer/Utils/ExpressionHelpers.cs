using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
                Lazy<ImmutableArray<ISymbol>> symbols = new Lazy<ImmutableArray<ISymbol>>(() =>
                {
                    var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccess);
                    return symbolInfo.Symbol != null ? ImmutableArray.Create(symbolInfo.Symbol) : symbolInfo.CandidateSymbols;
                });
                for (int i = 0; i < candidates.Length; i++)
                {
                    if (text == candidates[i].Name || text.EndsWith($".{candidates[i].Name}"))
                    {
                        if(symbols.Value.Any(symbol => symbol.Name == candidates[i].Name && symbol.ContainingType.ToString() == candidates[i].TypeFullName))
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
