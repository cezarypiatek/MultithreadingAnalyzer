using Microsoft.CodeAnalysis;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    internal static class SymbolHelper
    {
        public static bool IsPrimitiveType(ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Boolean:
                case SpecialType.System_Byte:
                case SpecialType.System_Char:
                case SpecialType.System_Double:
                case SpecialType.System_Int16:
                case SpecialType.System_Int32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt16:
                case SpecialType.System_UInt32:
                case SpecialType.System_UInt64:
                case SpecialType.System_IntPtr:
                case SpecialType.System_UIntPtr:
                case SpecialType.System_SByte:
                case SpecialType.System_Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanBeAssignedTo(ITypeSymbol symbolInfoType, string baseType)
        {
            if (symbolInfoType == null)
            {
                return false;
            }

            if (symbolInfoType.ToString() == baseType)
            {
                return true;
            }
            return CanBeAssignedTo(symbolInfoType.BaseType, baseType);
        }
    }
}