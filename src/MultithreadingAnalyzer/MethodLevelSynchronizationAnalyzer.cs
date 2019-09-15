using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodLevelSynchronizationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MT1010";
        internal static readonly LocalizableString Title = "Method level synchronization";
        internal static readonly LocalizableString MessageFormat = "Method level synchronization acquires lock on the whole instance or type and may cause a deadlock.";
        internal const string Category = "Locking";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeMethodAttributes, SyntaxKind.Attribute);
        }
        
        private void AnalyzeMethodAttributes(SyntaxNodeAnalysisContext context)
        {
            var attributeStatement = (AttributeSyntax)context.Node;

            if (attributeStatement == null || attributeStatement.Name.TryGetInferredMemberName() != "MethodImpl")
            {
                return;
            }

            var argumentExpression = attributeStatement.ArgumentList?.Arguments.FirstOrDefault().Expression;
            TryReportViolation(context, argumentExpression);
        }

        private void TryReportViolation(SyntaxNodeAnalysisContext context, ExpressionSyntax firstArgumentExpression)
        {
            if (firstArgumentExpression is MemberAccessExpressionSyntax argumentValue && argumentValue.ToFullString().Trim() == "MethodImplOptions.Synchronized")
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, argumentValue.GetLocation()));
                
            }
            else if(firstArgumentExpression is BinaryExpressionSyntax binaryExpression)
            {
                TryReportViolation(context, binaryExpression.Left);
                TryReportViolation(context, binaryExpression.Right);
            }
        }
    }
}
