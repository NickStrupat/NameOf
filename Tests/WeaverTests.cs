using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;
using NameOf.Fody;

namespace Tests {
    [TestClass]
    public class WeaverTests {
        [TestMethod]
        public void Setup() {
            var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
            var assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
            assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif
            var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
            var moduleWeaver = new ModuleWeaver { ModuleDefinition = moduleDefinition };
            //moduleWeaver.ModuleDefinition.ReadSymbols();
            moduleWeaver.Execute();
            moduleWeaver.ModuleDefinition.Write(assemblyPath);
        }
    }
}
