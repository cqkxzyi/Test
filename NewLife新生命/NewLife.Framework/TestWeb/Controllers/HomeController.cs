using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCode.DataAccessLayer;

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // 启用控制台日志
            XTrace.UseConsole();

            Test();

            return View();
        }

        private void Test() {

            SysBrowsinglog log = new SysBrowsinglog()
            {
                BigType = 2,
                Ip = "127.0.0.1",
                OperationID = 123,
                OperationTime = DateTime.Now
            };
            int result = log.Save();
         

            var model = SysBrowsinglog.Find(SysBrowsinglog._.ID==1);
            model.OperationTime = DateTime.Now;
            model.Save();


            var dal = DAL.Create("MSSQL");
            var db = dal.Query("select * from SYS_BrowsingLog where id>2");
            var list = SysBrowsinglog.LoadData(db);



        }


    }
}