using System.Reactive;

namespace Deploy.service.api;

public interface ServerService
{
    /// <summary>
    /// Start the server, then emit an event when it started.
    /// </summary>
    /// <returns>An observable that emits 1 event when the server is on.</returns>
    public IObservable<Unit> Start(string serverPath);
    public void Stop(string serverPath);

    /// <summary>
    /// Copy the JAR into serverPath/lib/extensions/
    /// and delete the other JARs with the same name.
    /// </summary>
    public void UpdateJar(string serverPath, string jarPath);
}