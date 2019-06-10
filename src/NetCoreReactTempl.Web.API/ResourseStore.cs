using Microsoft.Extensions.Localization;

namespace NetCoreReactTempl.Web.API
{
    public class ResourseStore
    {
        private readonly IStringLocalizer<ResourseStore> _localizer;
        public ResourseStore(IStringLocalizer<ResourseStore> localizer)
        {
            _localizer = localizer;
        }
    }
}
