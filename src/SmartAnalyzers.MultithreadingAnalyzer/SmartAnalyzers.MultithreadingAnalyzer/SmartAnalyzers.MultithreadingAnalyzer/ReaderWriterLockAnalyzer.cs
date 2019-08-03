using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ReaderWriterLockAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MT1006";
        internal static readonly LocalizableString Title = "Replace ReaderWriterLock with ReaderWriterLockSlim";
        internal static readonly LocalizableString MessageFormat = "Consider using ReaderWriterLockSlim instead of ReaderWriterLock in order to avoid potential deadlocks";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

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
            TryToReportViolation(context, fieldDeclaration, fieldDeclaration.Type);
        }

        private void AnalyzeField(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclaration = (FieldDeclarationSyntax)context.Node;
            TryToReportViolation(context, fieldDeclaration, fieldDeclaration.Declaration.Type);
        }

        private static void TryToReportViolation(SyntaxNodeAnalysisContext context, SyntaxNode memberDeclaration,
            TypeSyntax fieldDeclarationType)
        {
            if (fieldDeclarationType.ToFullString().Trim().EndsWith("ReaderWriterLock"))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, memberDeclaration.GetLocation()));
            }
        }
    }
}
