using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// 命令行帮助类
    /// </summary>
    public static class Cmder
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="fileName">可执行程序路径</param>
        /// <param name="args">命令参数</param>
        /// <returns></returns>
        public static string Run(string fileName, string args)
        {
            var process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = args;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return result;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令行</param>
        /// <returns></returns>
        public static string Run(string command)
        {
            var process = new Process();
            process.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";
            // process.StartInfo.Arguments = $"-c \"{command}\"";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine(command);
            process.StandardInput.Close();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return result;
        }
    }
}