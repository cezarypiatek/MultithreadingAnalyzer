using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public partial class LockObjectSelectionAnalyzer : BasicLockAnalyzer
    {
        const string Category = "Locking";
        public static readonly DiagnosticDescriptor MT1000 = new DiagnosticDescriptor(nameof(MT1000), "Lock on publicly accessible member", "Locking on publicly accessible member can cause a deadlock'", Category, DiagnosticSeverity.Error, true);
        public static readonly DiagnosticDescriptor MT1001 = new DiagnosticDescriptor(nameof(MT1001), "Lock on this reference", "Locking on this reference can cause a deadlock", Category, DiagnosticSeverity.Error, true);
        public static readonly DiagnosticDescriptor MT1002 = new DiagnosticDescriptor(nameof(MT1002), "Lock on object with weak identity", "Locking on object with weak identity can cause a deadlock because their are implicitly shared across the application", Category, DiagnosticSeverity.Error, true);
        public static readonly DiagnosticDescriptor MT1003 = new DiagnosticDescriptor(nameof(MT1003), "Lock on non-readonly member", "Locking on non-readonly member can cause a deadlock'", Category, DiagnosticSeverity.Error, true);
        public static readonly DiagnosticDescriptor MT1004 = new DiagnosticDescriptor(nameof(MT1004), "Lock on value type instance", "Locking on value types prohibits any synchronization because they are boxed", Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
            ImmutableArray.Create(MT1001, MT1000, MT1003, MT1004, MT1002);

        protected override void TryToReportViolation(SyntaxNodeAnalysisContext context, ExpressionSyntax expression)
        {
            if (expression == null)
            {
                return;
            }

            var expressionKind = expression.Kind();

            if (IsLockOnThisReferenceViolated(expressionKind))
            {
                ReportViolation(context, expression, MT1001);
                return;
            }

            var symbolInfo = context.SemanticModel.GetSymbolInfo(expression);

            if (IsLockOnNonReadonlyViolated(expressionKind, symbolInfo))
            {
                ReportViolation(context, expression, MT1003);
            }

            if (IsLockPubliclyAccessibleMemberViolated(expressionKind, symbolInfo))
            {
                ReportViolation(context, expression, MT1000);
            }

            var typeInfo = context.SemanticModel.GetTypeInfo(expression);

            if (IsLockOnValueTypeViolated(typeInfo))
            {
                ReportViolation(context, expression, MT1004);
                return;
            }

            if (IsLockOnObjectWithWeakIdentity(typeInfo))
            {
                ReportViolation(context, expression, MT1002);
                return;
            }
        }

        private bool IsLockOnObjectWithWeakIdentity(TypeInfo typeInfo)
        {
            switch (typeInfo.Type.TypeKind)
            {
                case TypeKind.Array:
                    return typeInfo.Type is IArrayTypeSymbol arrayType && SymbolHelper.IsPrimitiveType(arrayType.ElementType);
                case TypeKind.Class:
                case TypeKind.TypeParameter:
                    return
                        typeInfo.Type.SpecialType == SpecialType.System_String || 
                        SymbolHelper.CanBeAssignedTo(typeInfo.Type, "System.Exception") || 
                        SymbolHelper.CanBeAssignedTo(typeInfo.Type, "System.MarshalByRefObject") || 
                        SymbolHelper.CanBeAssignedTo(typeInfo.Type, "System.Reflection.MemberInfo") || 
                        SymbolHelper.CanBeAssignedTo(typeInfo.Type, "System.Reflection.ParameterInfo") || 
                        SymbolHelper.CanBeAssignedTo(typeInfo.Type, "System.Runtime.ConstrainedExecution.CriticalFinalizerObject");
                default:
                    return false;
            }
        }

        private static bool IsLockOnValueTypeViolated(TypeInfo typeInfo)
        {
            return typeInfo.Type != null && typeInfo.Type.IsValueType;
        }

        private static void ReportViolation(SyntaxNodeAnalysisContext context, ExpressionSyntax expression, DiagnosticDescriptor rule)
        {
            context.ReportDiagnostic(Diagnostic.Create(rule, expression.GetLocation()));
        }

        private static bool IsLockPubliclyAccessibleMemberViolated(SyntaxKind expressionKind, SymbolInfo symbolInfo)
        {
            if (expressionKind == SyntaxKind.SimpleMemberAccessExpression || expressionKind == SyntaxKind.IdentifierName)
            {
                if (symbolInfo.Symbol is IPropertySymbol || symbolInfo.Symbol is IFieldSymbol ||
                    symbolInfo.Symbol is IEventSymbol)
                {
                    if (symbolInfo.Symbol.DeclaredAccessibility != Accessibility.Private)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsLockOnThisReferenceViolated(SyntaxKind expressionKind)
        {
            return expressionKind == SyntaxKind.ThisExpression;
        }

        private static bool IsLockOnNonReadonlyViolated(SyntaxKind expressionKind, SymbolInfo symbolInfo)
        {
            if (expressionKind == SyntaxKind.SimpleMemberAccessExpression || expressionKind == SyntaxKind.IdentifierName)
            {
                if ((symbolInfo.Symbol is IFieldSymbol fieldSymbol && fieldSymbol.IsReadOnly == false) || 
                    (symbolInfo.Symbol is IPropertySymbol propertySymbol && propertySymbol.IsReadOnly == false))
                {
                    return true;
                }
            }

            return false;
        }
    }
}