using System.Collections.Generic;

namespace NetCoreReactTempl.Domain.Models
{
    public class BaseData
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        public Dictionary<string, object> Fields { get; set; }
    }
}
