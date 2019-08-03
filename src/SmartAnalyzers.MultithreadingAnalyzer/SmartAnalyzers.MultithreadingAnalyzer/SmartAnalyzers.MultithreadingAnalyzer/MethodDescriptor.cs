using System;

namespace SmartAnalyzers.MultithreadingAnalyzer
{
    class MethodDescriptor
    {
        public string FullName { get; }

        public MethodDescriptor(string fullName)
        {
            FullName = fullName;
            var parts = fullName.Split('.');
            this.Name = parts[parts.Length-1];
            this.TypeFullName = String.Join(".", parts, 0, parts.Length - 1);
        }
        
        public string TypeFullName { get; }

        public string Name { get; }
    }
}
