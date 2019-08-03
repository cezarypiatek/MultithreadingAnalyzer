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
    public class BoxedSpinLockAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MT1005";
        internal static readonly LocalizableString Title = "Boxed SpinLock is useless";
        internal static readonly LocalizableString MessageFormat = "SpinLock is a struct so `readonly` cause boxing which makes SpinLock useless.";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.RegisterSyntaxNodeAction(AnalyzeField, SyntaxKind.FieldDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzeProperty(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclaration = (PropertyDeclarationSyntax)context.Node;
            TryToReportViolation(context, fieldDeclaration.Modifiers, fieldDeclaration, fieldDeclaration.Type);
        }

        private void AnalyzeField(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclaration = (FieldDeclarationSyntax)context.Node;
            TryToReportViolation(context, fieldDeclaration.Modifiers, fieldDeclaration, fieldDeclaration.Declaration.Type);
        }

        private static void TryToReportViolation(SyntaxNodeAnalysisContext context, SyntaxTokenList modifiers, SyntaxNode memberDeclaration, TypeSyntax fieldDeclarationType)
        {
            if (fieldDeclarationType.ToFullString().Trim().EndsWith("SpinLock") &&  modifiers.Any(x => x.Kind() == SyntaxKind.ReadOnlyKeyword))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, memberDeclaration.GetLocation()));
            }
        }
    }
}
