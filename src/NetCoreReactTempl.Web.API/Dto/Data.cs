using System.Collections.Generic;

namespace NetCoreReactTempl.Web.API.Dto
{
    public class Data : BaseDto
    {
        public long UserId { get; set; }

        public Dictionary<string, object> Fields { get; set; }
    }
}
