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
        internal const string Category = "Locking";

        public static DiagnosticDescriptor MT1014 = new DiagnosticDescriptor(nameof(MT1014), "Passed by value SpinLock is useless", (LocalizableString) "SpinLock is a struct and passing it by value results with copy which makes SpinLock useless.", Category, DiagnosticSeverity.Error, true);
        public static DiagnosticDescriptor MT1015 = new DiagnosticDescriptor(nameof(MT1015), "Readonly SpinLock is useless", "SpinLock is a struct so `readonly` cause boxing which makes SpinLock useless.", Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MT1014, MT1015);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeParameter, SyntaxKind.Parameter);
            context.RegisterSyntaxNodeAction(AnalyzeField, SyntaxKind.FieldDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzeParameter(SyntaxNodeAnalysisContext context)
        { 
            var parameter = (ParameterSyntax)context.Node;

            if (parameter.Type == null)
            {
                return;
            }

            if (parameter.Type.ToFullString().Trim().EndsWith("SpinLock") && parameter.Modifiers.Any(IsRefOrOutModifier) == false)
            {
                context.ReportDiagnostic(Diagnostic.Create(MT1014, parameter.GetLocation()));
            }
        }

        private static bool IsRefOrOutModifier(SyntaxToken x)
        {
            var kind = x.Kind();
            return kind == SyntaxKind.RefKeyword || kind == SyntaxKind.OutKeyword;
        }

        private void AnalyzeProperty(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclaration = (PropertyDeclarationSyntax)context.Node;
            TryToReportSpinlockPassAsValue(context, fieldDeclaration.Modifiers, fieldDeclaration, fieldDeclaration.Type);
        }

        private void AnalyzeField(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclaration = (FieldDeclarationSyntax)context.Node;
            TryToReportSpinlockPassAsValue(context, fieldDeclaration.Modifiers, fieldDeclaration, fieldDeclaration.Declaration.Type);
        }

        private static void TryToReportSpinlockPassAsValue(SyntaxNodeAnalysisContext context, SyntaxTokenList modifiers, SyntaxNode memberDeclaration, TypeSyntax fieldDeclarationType)
        {
            if (fieldDeclarationType.ToFullString().Trim().EndsWith("SpinLock") && modifiers.Any(x => x.Kind() == SyntaxKind.ReadOnlyKeyword))
            {
                context.ReportDiagnostic(Diagnostic.Create(MT1015, memberDeclaration.GetLocation()));
            }
        }
    }
}
