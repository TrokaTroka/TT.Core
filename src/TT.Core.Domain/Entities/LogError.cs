namespace TT.Core.Domain.Entities;

public class LogError : EntityBase
{
    public LogError(string service, string method, string error)
    {
        Service = service;
        Method = method;
        Error = error;
    }

    public string Service { get; private set; }
    public string Method { get; private set; }
    public string Error { get; private set; }
}
