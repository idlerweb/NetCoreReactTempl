namespace NetCoreReactTempl.Domain.Configuration
{
    public interface IConfigurationStore
    {
        string AuthSecret { get; }
    }
}
