namespace HttpFaultProxy.Model.Proxies
{
    public interface IProxyProvider
    {
        IProxy Get(string uri);
    }
}