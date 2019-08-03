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
    public class CopiedSpinLockAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MT1004";
        internal static readonly LocalizableString Title = "Passed by value SpinLock is useless";
        internal static readonly LocalizableString MessageFormat = "SpinLock is a struct and passing it by value results with copy which makes SpinLock useless.";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.RegisterSyntaxNodeAction(AnalyzeParameter, SyntaxKind.Parameter);
        }

        private void AnalyzeParameter(SyntaxNodeAnalysisContext context)
        {
            var parameter = (ParameterSyntax)context.Node;
            if (parameter.Type.ToFullString().Trim().EndsWith("SpinLock") && parameter.Modifiers.Any(x => x.Kind() == SyntaxKind.RefKeyword) == false)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, parameter.GetLocation()));
            }
        }
    }
}
