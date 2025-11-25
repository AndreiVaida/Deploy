namespace Deploy.logger;

public interface Logger
{
    void Info(string message);
    void Warning(string message);
    void Error(string message, Exception exception);
}