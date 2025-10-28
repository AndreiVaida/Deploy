namespace Deploy.service.api;

public interface WindowService
{
    public void KillProcess(string processName);
    public void KillCmdProcess(string serverName, string windowName);
}