using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace NetCoreReactTempl.Web.API
{
    public class ConfigurationStore
    {
        readonly Dictionary<string, string> _configuration;

        public ConfigurationStore(IOptions<Dictionary<string, string>> configuration) => _configuration = configuration.Value;

        public string AuthSecret => _configuration["AuthSecret"];
    }
}
