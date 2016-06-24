/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hwly.Info.Model;
using Hwly.Info.UI.MVC;
using Hwly.Info.BLL;
using System.IO;

namespace Hwly.Info.UI.MVC
{
    public class ErrorAttribute : FilterAttribute, IExceptionFilter
    {
        static string isError = System.Configuration.ConfigurationManager.AppSettings["isError"];
        public void OnException(ExceptionContext filterContext)
        {
            if (!bool.Parse(isError))
            { return; }
            string type = filterContext.HttpContext.Request.Headers["x-requested-with"];
            S_ErrorLog info = new S_ErrorLog();
            info.ErrMsg = filterContext.Exception.Message;
            object areaObj = filterContext.RouteData.Values["area"];
            string area = "";
            if (areaObj != null) { area = areaObj.ToString(); }
            if (area == "")
            {
                area = MVCHelp.GetAreaName();
            }
            string action = filterContext.RouteData.Values["action"].ToString();
            action = string.IsNullOrEmpty(action) ? "Index" : action;
            info.ErrUrl = area + "/" + filterContext.RouteData.Values["controller"] + "/" + action;
            info.LoginName = Common.CommInfo.GetLoginName();
            var bll = new BLLSYSErrorLog();
            int id = 0;
            try
            {
                 id=  bll.Insert(info);
            }
            catch (Exception e)
            {
                string filepath = HttpContext.Current.Server.MapPath("~/log/templog.txt");
                if (!File.Exists(filepath))
                    File.Create(filepath);
                StreamWriter writer = File.AppendText(filepath);
                writer.WriteLine(DateTime.Now.ToString("日志记录错误yyyy-MM-dd HH:mm:ss") + " " + e.Message );
                writer.Flush();
                writer.Close();  
            }
            finally
            {
                string msg = "";
                Exception ex = filterContext.Exception;
                ErrorMsg exxx = ex as ErrorMsg;
                string name = ex.GetType().Name;
                if (name == "ErrorMsg")
                {
                    msg = ex.Message;
                }
                else
                {
                    msg = "不好意思，系统好像出了点问题！<br/>请联系系统管理员：错误编号" + id;
                }

                if (type != null && type.ToLower() == "xmlhttprequest")//ajax请求
                {
                    filterContext.Result = MVCHelp.GetErrorMsg(1, msg);
                }
                else//同步请求
                {
                    filterContext.Controller.TempData["errorMsg"] = msg;
                    filterContext.Result = MVCHelp.RedirectToAction("Public", "Error", "");
                }
            }

            filterContext.ExceptionHandled = true;
        }
    }
}

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Hwly.Info.UI.MVC
{
    public class ErrorAttribute : FilterAttribute, IExceptionFilter
    {
       
        public void OnException(ExceptionContext filterContext)
        {
           
            string type = filterContext.HttpContext.Request.Headers["x-requested-with"];
            if (type != null && type.ToLower() == "xmlhttprequest")//ajax请求
            {
                filterContext.Result = MVCHelp.GetErrorMsg(1, filterContext.Exception.Message);
            }
            else//同步请求
            {
                filterContext.Controller.TempData["errorMsg"] = filterContext.Exception.Message;
                filterContext.Result = MVCHelp.RedirectToAction("Home", "Error", "");
            }
         
            filterContext.ExceptionHandled = true;
        }
    }
}