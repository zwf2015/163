using System.Web;
using System.Web.Mvc;

namespace LocalAccounts
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //7.给项目启用SSL，向 MVC 管道添加过滤器。
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
