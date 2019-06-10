using System.Collections.Generic;

namespace NetCoreReactTempl.Web.API.Handlers.Abstractions
{
    public class BaseResponse<T> where T : Dto.BaseDto
    {
        public readonly T Data;
        public readonly List<T> List;
        public readonly int Count;

        public BaseResponse(T data, List<T> list, int count) {
            Data = data;
            List = list;
            Count = count;
        }
    }
}
