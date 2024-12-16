using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using Deploy.service.api;

namespace Deploy.service.impl;

public class WindowServiceImpl : WindowService
{
    private const uint CtrlCEvent = 0;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AttachConsole(uint dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FreeConsole();

    [DllImport("kernel32.dll")]
    static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

    public void KillProcess(string processName)
    {
        var process = Process.GetProcessesByName(processName).FirstOrDefault();
        if (process != null)
            TerminateProcess(process.Handle, 0);
    }

    public void KillCmdProcess(string cmdFile)
    {
        var processes = Process.GetProcessesByName("cmd");
        var process = processes.FirstOrDefault(process => {
            var commandLine = GetCommandLine(process.Id);
            return IsSameProcess(commandLine, cmdFile);
        });

        if (process != null) KillCmdProcess(process);
    }

    private static bool IsSameProcess(string commandLine, string batchFilePath) => commandLine.Contains(batchFilePath);

    private static void KillCmdProcess(Process process)
    {
        AttachConsole((uint) process.Id);
        GenerateConsoleCtrlEvent(CtrlCEvent, 0);
        FreeConsole();
        Thread.Sleep(500);
        TerminateProcess(process.Handle, 0);
    }

    private static string GetCommandLine(int processId)
    {
        using var searcher = new ManagementObjectSearcher($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {processId}");
        foreach (var @object in searcher.Get())
            return @object["CommandLine"].ToString()!;

        return null;
    }
}