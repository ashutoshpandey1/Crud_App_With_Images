using System.Web;
using System.Web.Mvc;

namespace Crud_App_With_Images
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
