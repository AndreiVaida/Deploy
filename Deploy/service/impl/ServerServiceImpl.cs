using System.Reactive;
using System.Reactive.Linq;
using Deploy.service.api;

namespace Deploy.service.impl;

internal class ServerServiceImpl : ServerService
{
    public IObservable<Unit> Start(string serverPath)
    {
        return Observable.FromAsync(() =>
        {
            return Task.FromResult(Unit.Default);
        });
    }

    public void Stop(string serverPath)
    {
    }

    public void UpdateJar(string serverPath, string jarPath)
    {
    }
}