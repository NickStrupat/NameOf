using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    public static class Verifier
    {
        public static void Verify(string assemblyPath2)
        {

            var exePath = GetPathToPeVerify();        
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

        private static string GetPathToPeVerify()
        {
            //return Path.Combine(ToolLocationHelper.GetPathToDotNetFrameworkSdk(TargetDotNetFrameworkVersion.Version40), @"bin\NETFX 4.0 Tools\peverify.exe");
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var windowsSdkDirectory = Path.Combine(programFilesPath, @"Microsoft SDKs\Windows");
            if (!Directory.Exists(windowsSdkDirectory))
                throw new FileNotFoundException("peverify.exe not found.");
            var peVerifyPath = Directory.EnumerateFiles(windowsSdkDirectory, "peverify.exe", SearchOption.AllDirectories)
                .Where(x => !x.ToLowerInvariant().Contains("x64"))
                .OrderByDescending(x => FileVersionInfo.GetVersionInfo(x).FileVersion)
                .FirstOrDefault();
            if (peVerifyPath == null)
                throw new FileNotFoundException("peverify.exe not found.");
            return peVerifyPath;
        }
    }
}