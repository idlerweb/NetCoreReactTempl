using System.Collections.Generic;

namespace NetCoreReactTempl.Domain.Models
{
    public class Data : BaseModel
    {
        public long UserId { get; set; }

        public Dictionary<string, string> Fields { get; set; }
    }
}
