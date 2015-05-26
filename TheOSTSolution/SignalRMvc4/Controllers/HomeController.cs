using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

namespace SignalRMvc4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Web接口，推送消息
        /// </summary>
        /// <returns></returns>
        public string PushMsg(string msg)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext("pushHub");
            context.Clients.All.addMessage(msg);
            return "ok";
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}
