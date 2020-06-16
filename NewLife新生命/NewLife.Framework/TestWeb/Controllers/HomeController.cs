using NewLife.Collections;
using NewLife.Log;
using NewLife.Security;
using NewLife.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCode;
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

           
            var model = SysBrowsinglog.Find(SysBrowsinglog._.ID==1);
            model.OperationTime = DateTime.Now;
            model.Save();

            model = SysBrowsinglog.FindByKey("111");
            model = SysBrowsinglog.FindByKey(1);


            //where参数化
            var exp =new WhereExpression();
            exp &= SysBrowsinglog._.ID > 0;
            exp &= SysBrowsinglog._.OperationTime > "2020-01-01";
            exp += SysBrowsinglog._.ID.GroupBy();
            exp.GroupBy(SysBrowsinglog._.ID);
            SysBrowsinglog.FindAll(exp,null,"ID");


            var dal = DAL.Create("MSSQL");
            
            var db = dal.Query("select * from SYS_BrowsingLog where id>2");
            var list = SysBrowsinglog.LoadData(db);

            var a = Pool.StringBuilder.Get();
            a.Separate(",").Append("{0}={1}".F());


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
                        Ip = "123.325.3.3"
                    };
                    add.Insert();

                    //return;
                    //throw new Exception("保存时，异常！");
                   
                    SysBrowsinglog add2 = new SysBrowsinglog()
                    {
                        Ip = "123.325.4.4"
                    };
                    add2.Insert();


                    tran1.Commit();

                }
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
            
        }

        public void TestCache() {
            SysBrowsinglog.Meta.Session.Dal.Session.ShowSQL = false;

            var ids = Enumerable.Range(0, 20).Select(e => Rand.Next(100)).ToList();
            var count = 1000000;
            var sw = Stopwatch.StartNew();
            foreach (var item in ids)
            {
                var ent = SysBrowsinglog.FindByID(item);

            }
            sw.Stop();

            var ms = sw.Elapsed.TotalSeconds;

        }


    }
}