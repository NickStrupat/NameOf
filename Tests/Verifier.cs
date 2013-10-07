using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    public static class Verifier
    {
        public static void Verify(string assemblyPath2)
        {

            var exePath = GetPathToPEVerify();        
            var process = Process.Start(new ProcessStartInfo(exePath, "\"" + assemblyPath2 + "\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            process.WaitForExit(10000);
            var readToEnd = process.StandardOutput.ReadToEnd().Trim();
            Assert.IsTrue(readToEnd.Contains(String.Format("All Classes and Methods in {0} Verified.", assemblyPath2)), readToEnd);
        }

        private static string GetPathToPEVerify()
        {
            return Path.Combine(ToolLocationHelper.GetPathToDotNetFrameworkSdk(TargetDotNetFrameworkVersion.Version40), @"bin\NETFX 4.0 Tools\peverify.exe");
        }
    }
}