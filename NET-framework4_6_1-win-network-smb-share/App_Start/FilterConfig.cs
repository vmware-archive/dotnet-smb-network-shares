using System.Web;
using System.Web.Mvc;

namespace NET_framework4_6_1_win_network_smb_share
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
