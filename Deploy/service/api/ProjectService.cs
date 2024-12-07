namespace Deploy.service.api;

public interface ProjectService
{
    /// <summary>
    /// Run `gradlew assemble` on the provided path, then returns the path of the generated jar.
    /// </summary>
    /// <returns>An observable that emits 1 item: the path to the generated jar.</returns>
    public IObservable<string> Build(string projectPath);
}