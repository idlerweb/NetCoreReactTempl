using System.Collections.Generic;
using System.Linq;

namespace NetCoreReactTempl.App
{
    public class RestResponseBase<T> where T : class
    {
        public T Data { get; }
        public IEnumerable<T> List { get; }

        public int Count => List?.Count() ?? 1;

        public RestResponseBase(T data = null, IEnumerable<T> list = null)
        {
            Data = data;
            List = list;            
        }
    }
}
