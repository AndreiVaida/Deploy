using System.Diagnostics;

namespace Deploy.utils;

public static class ExecUtils
{
    public static Process RunCommand(string pathOfExecution, string command, bool waitToFinish = true)
    {
        var process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/C cd {pathOfExecution} && {command}";
        process.StartInfo.UseShellExecute = false;

        process.Start();
        if (waitToFinish) process.WaitForExit();
        return process;
    }
}