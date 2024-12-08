﻿using System.Reactive;

namespace Deploy.service.api;

public interface ServerService
{
    /// <summary>
    /// Start the server, then emit an event when it started.
    /// </summary>
    /// <returns>An observable that emits 1 event when the server is on.</returns>
    public IObservable<Unit> Start(string serverPath);
    public void Stop(string serverPath);
    public void UpdateJar(string serverPath, string jarPath);
}