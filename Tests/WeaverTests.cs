using System;
using System.IO;
using AssemblyToProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;
using NameOf.Fody;

namespace Tests {
    [TestClass]
    public class WeaverTests {
        [TestMethod]
        public void ActualWeaving() {
            var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
            var assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
            assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif
            var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
            moduleDefinition.Assembly.MainModule.ReadSymbols();
            var moduleWeaver = new ModuleWeaver { ModuleDefinition = moduleDefinition };
            moduleWeaver.Execute();
            moduleWeaver.ModuleDefinition.Write(assemblyPath.Replace(".dll", ".weaved.dll"));
        }
        [TestMethod]
        public void Arguments() {
            Invocations.Arguments();
        }
        [TestMethod]
        public void Locals() {
            Invocations.Local();
        }
        [TestMethod]
        public void Types() {
            Invocations.Type();
        }
        [TestMethod]
        public void Statics() {
            Invocations.Static();
        }
        [TestMethod]
        public void StaticInstances() {
            Invocations.StaticInstance();
        }
        [TestMethod]
        public void Instances() {
            Invocations.Instance();
        }
        [TestMethod]
        public void Errors() {
            Invocations.Errors();
        }
    }
}
