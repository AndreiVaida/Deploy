using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using Deploy.service.api;

namespace Deploy.service.impl;

public class WindowServiceImpl : WindowService
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    private const uint WM_KEYDOWN = 0x0100;
    private const uint WM_KEYUP = 0x0101;
    private const int VK_RETURN = 0x0D;

    public void KillProcess(string processName)
    {
        var process = Process.GetProcessesByName(processName).FirstOrDefault();
        if (process != null)
            TerminateProcess(process.Handle, 0);
    }

    public void KillCmdProcess(string serverName, string windowName)
    {
        var processes = Process.GetProcessesByName("javaw");
        var process = processes.FirstOrDefault(process => {
            var commandLine = GetCommandLine(process.Id);
            return IsSameProcess(commandLine, serverName);
        });
        if (process == null) return;
        
        process.Kill();
        process.WaitForExit();
        CloseCmdWindow(windowName);
    }

    private static bool IsSameProcess(string commandLine, string batchFilePath) => commandLine.Contains(batchFilePath, StringComparison.CurrentCultureIgnoreCase);

    private static void CloseCmdWindow(string windowTitle)
    {
        var hWnd = FindWindow(null, windowTitle);
        if (hWnd == IntPtr.Zero) return;

        PostMessage(hWnd, WM_KEYDOWN, VK_RETURN, IntPtr.Zero);
        Thread.Sleep(500);
        PostMessage(hWnd, WM_KEYUP, VK_RETURN, IntPtr.Zero);
    }

    private static string GetCommandLine(int processId)
    {
        using var searcher = new ManagementObjectSearcher($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {processId}");
        foreach (var @object in searcher.Get())
            return @object["CommandLine"].ToString()!;

        return null;
    }
}