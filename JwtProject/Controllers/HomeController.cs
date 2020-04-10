using JWT;
using JwtProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace JwtProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Token()
        {
            

            var payload = new Dictionary<string, object>
                {
                    { "username","admin" },
                    { "pwd", "123" }
                };

            var token = JwtHelper.SetJwtEncode(payload);

            Thread.Sleep(5000);
            ViewBag.Token = token;
            var user = JwtHelper.GetJwtDecode(token);
            return View();
        }

        [HttpPost]
        public string Login(string token)
        {
            string login = EncryptionAlgorithm.GetStringFromBase64(token);
            var arr = login.Split('.');
            string uid = arr[0].GetStringFromBase64();
            string pwd = arr[1].GetStringFromBase64();
            return "";
        }
    }
}