using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using Deploy.logger;
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

    private readonly Logger _logger = new LoggerImpl(nameof(WindowServiceImpl));

    public void KillProcess(string processName)
    {
        var process = Process.GetProcessesByName(processName).FirstOrDefault();
        if (process == null)
        {
            _logger.Info($"Process '{processName}' not found, cannot kill");
            return;
        }

        _logger.Info($"Kill process '{processName}': Name='{process.ProcessName}', Handle='{process.Handle}'");
        TerminateProcess(process.Handle, 0);
        _logger.Info($"Process {processName} was killed");
    }

    public void KillCmdProcess(string serverName, string windowName)
    {
        _logger.Info($"Kill CMD process. ServerName='{serverName}', WindowName='{windowName}'");

        var processes = Process.GetProcessesByName("javaw");
        var process = processes.FirstOrDefault(process => {
            var commandLine = GetCommandLine(process.Id);
            return IsSameProcess(commandLine, serverName);
        });
        if (process == null)
        {
            _logger.Info($"CMD process not killed for server '{serverName}'. Not found.");
            return;
        }
        
        process.Kill();
        process.WaitForExit();
        CloseCmdWindow(windowName);

        _logger.Info($"CMD process for server '{serverName}' was killed");
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