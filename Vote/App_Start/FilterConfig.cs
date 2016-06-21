using System.Web;
using System.Web.Mvc;

namespace HWVote
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //aaa
            filters.Add(new HandleErrorAttribute());
        }
    }
}