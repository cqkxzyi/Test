using NewLife.Log;
using NewLife.Serialization;
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

            //SysBrowsinglog log = new SysBrowsinglog()
            //{
            //    BigType = 2,
            //    Ip = "127.0.0.1",
            //    OperationID = 123,
            //    OperationTime = DateTime.Now
            //};
            //int result = log.Save();


           var model = SysBrowsinglog.Find(SysBrowsinglog._.Remark,"'");

            var dal = DAL.Create("MSSQL");
            var db = dal.Query("select * from SYS_BrowsingLog where id>2");
            var list = SysBrowsinglog.LoadData(db);

            //表结构
            var tables=dal.Tables;
            string tables_str = tables.ToJson(true);
            XTrace.WriteLine(tables_str);

            //事务
            try
            {
                using (var tran1 = SysBrowsinglog.Meta.CreateTrans())
                {
                    SysBrowsinglog add = new SysBrowsinglog()
                    {
                        Ip = "123.325.3.99"
                    };
                    add.Insert();

                    //return;
                    //throw new Exception("保存时，异常！");
                   
                    SysBrowsinglog add2 = new SysBrowsinglog()
                    {
                        Ip = "123.325.4.44"
                    };
                    add2.Insert();


                    tran1.Commit();

                }
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }

            //高级查询
            list = SysBrowsinglog.Search();

        }


    }
}