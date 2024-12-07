using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using Deploy.service.api;

namespace Deploy.service.impl;

public class ProjectServiceImpl : ProjectService
{
    public IObservable<string> Build(string projectPath)
    {
        return Observable.Return(".jar");
    }
}