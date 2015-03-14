using System;
using System.IO;
using AssemblyToProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;
using NameOf.Fody;

namespace Tests {
    [TestClass]
    public class Weaver {
        [TestMethod]
        public void ActualWeaving() {
            //var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
            //var assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
			var assemblyPath = @"AssemblyToProcess.dll";
#if (!DEBUG)
            assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif
            var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
            try {
                moduleDefinition.Assembly.MainModule.ReadSymbols();
            }
            catch (InvalidOperationException ex) {
                throw new Exception("Make sure Mono.Cecil.Pdb and/or Mono.Cecil.Mdb is/are referenced.", ex);
            }
            var moduleWeaver = new ModuleWeaver { ModuleDefinition = moduleDefinition };
            moduleWeaver.Execute();
            moduleWeaver.ModuleDefinition.Write(assemblyPath/* = assemblyPath.Replace(".dll", ".weaved.dll")*/);
            Verifier.Verify(assemblyPath);
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
		public void AsyncAwait() {
			Invocations.AsyncAwait();
		}
		[TestMethod]
		public void Member() {
			Invocations.Member();
		}
		//[TestMethod]
		//public void Errors() {
		//	Invocations.Errors();
		//}
    }
}
