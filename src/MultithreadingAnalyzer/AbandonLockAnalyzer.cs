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
        internal const string Category = "Locking";

        internal static readonly DiagnosticDescriptor MT1012 = new DiagnosticDescriptor(nameof(MT1012), "Acquiring lock without guarantee of releasing", (LocalizableString) "Acquiring lock should always be wrapped in try block to ensure execution", Category, DiagnosticSeverity.Error, true);
        internal static readonly DiagnosticDescriptor MT1013 = new DiagnosticDescriptor(nameof(MT1013), "Releasing lock without guarantee of execution", (LocalizableString) "Releasing lock should always be wrapped in finally block to ensure execution", Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MT1012, MT1013);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            context.RegisterSyntaxNodeAction(AnalyzeMonitorMethodInvocation, SyntaxKind.InvocationExpression);
        }

        private static readonly MethodDescriptor[] MethodsThatRequireFinally = 
        {
            new MethodDescriptor("System.Threading.Monitor.Exit"),
            new MethodDescriptor("System.Threading.SpinLock.Exit"),
            new MethodDescriptor("System.Threading.Mutex.ReleaseMutex"),
            new MethodDescriptor("System.Threading.ReaderWriterLockSlim.ExitWriteLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLockSlim.ExitReadLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLockSlim.ExitUpgradeableReadLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLock.ReleaseReaderLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLock.ReleaseWriterLock"),
        };

        private static readonly MethodDescriptor[] MethodsThatRequireTry = 
        {
            new MethodDescriptor("System.Threading.Monitor.Enter"),
            new MethodDescriptor("System.Threading.Monitor.TryEnter"),
            new MethodDescriptor("System.Threading.SpinLock.Enter"),
            new MethodDescriptor("System.Threading.SpinLock.TryEnter"),
            new MethodDescriptor("System.Threading.Mutex.WaitOne"),
            new MethodDescriptor("System.Threading.ReaderWriterLockSlim.EnterWriteLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLockSlim.EnterReadLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLockSlim.EnterUpgradeableReadLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLock.AcquireReaderLock"),
            new MethodDescriptor("System.Threading.ReaderWriterLock.AcquireWriterLock"),
        };

        private void AnalyzeMonitorMethodInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;
            if (ExpressionHelpers.IsInvocationOf(context, MethodsThatRequireFinally))
            {
                TryReportLockReleaseWithoutExecutionGuarantee(context, invocationExpression.Parent, invocationExpression);
            }else if (ExpressionHelpers.IsInvocationOf(context, MethodsThatRequireTry))
            {
                TryReportLockAcquiredWithoutReleaseGuarantee(context, invocationExpression.Parent, invocationExpression);
            }
        }

        private void TryReportLockAcquiredWithoutReleaseGuarantee(SyntaxNodeAnalysisContext context, SyntaxNode parentSyntaxNode, InvocationExpressionSyntax acquiredExpression)
        {
            if (parentSyntaxNode is TryStatementSyntax)
            {
                return;
            }
            if (parentSyntaxNode is MethodDeclarationSyntax methodDeclaration && methodDeclaration.Parent is TypeDeclarationSyntax)
            {
                context.ReportDiagnostic(Diagnostic.Create(MT1012, acquiredExpression.GetLocation()));
            }
            else if (parentSyntaxNode.Parent != null)
            {
                TryReportLockAcquiredWithoutReleaseGuarantee(context, parentSyntaxNode.Parent, acquiredExpression);
            }
        }

        private void TryReportLockReleaseWithoutExecutionGuarantee(SyntaxNodeAnalysisContext context, SyntaxNode parentSyntaxNode, SyntaxNode releaseExpression)
        {
            if (parentSyntaxNode is FinallyClauseSyntax)
            {
                return;
            }
            if (parentSyntaxNode is MethodDeclarationSyntax methodDeclaration && methodDeclaration.Parent is TypeDeclarationSyntax)
            {
                context.ReportDiagnostic(Diagnostic.Create(MT1013, releaseExpression.GetLocation()));     
            }
            else if(parentSyntaxNode.Parent !=null)
            {
                TryReportLockReleaseWithoutExecutionGuarantee(context, parentSyntaxNode.Parent, releaseExpression);
            }
        }
    }
}
