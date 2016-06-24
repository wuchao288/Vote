using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections;
using System.Text;
using System.Net;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Drawing;

namespace Hwly.Info.UI.MVC
{
    public static class MVCHelp
    {
       
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static RedirectToRouteResult RedirectToAction(string controllerName, string actionName,string areaName)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd["controller"] = controllerName;
            rvd["action"] = actionName;
            rvd["area"] = areaName;
            RedirectToRouteResult rtr = new RedirectToRouteResult(rvd);
            return rtr;
        }

        public static RedirectToRouteResult RedirectToAction(string controllerName, string actionName, string areaName,string paraName,string paraValue)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd["controller"] = controllerName;
            rvd["action"] = actionName;
            if (!string.IsNullOrEmpty(areaName))
            {
                rvd["area"] = areaName;
            }
            else
            {
                rvd["area"] = "";
            }
            rvd[paraName] = paraValue;
            RedirectToRouteResult rtr = new RedirectToRouteResult(rvd);
            return rtr;
        }
       
        public static T GetJsonObject<T>(string value)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
           
            value = HttpContext.Current.Server.HtmlDecode(value);
            value = value.Replace("[and]", "&");
            return js.Deserialize<T>(value);
        }

        public static List<T> GetJsonList<T>(string value)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js.Deserialize<List<T>>(value);
        }

        public static ActionResult GetErrorMsg(int num, string msg)
        {
            JsonResult json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.Data = new { errorNum = num, msg = msg };
            return json;
        }

        public static string GetEasyUIData(System.Collections.IList list, int count)
        {
            var data = new { rows = list, total = count };
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js.Serialize(data);
        }

        public static string GetEasyUIDataNetSoft(System.Collections.IList list, int count)
        {
            var data = new { rows = list, total = count };
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd";
            return JsonConvert.SerializeObject(data, timeFormat);
        }
        public static string GetEasyUIDataNetSoft(System.Collections.IList list, int count,bool IsTime)
        {
            var data = new { rows = list, total = count };
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(data, timeFormat);
        }
        public static string GetJsObjNetSoft(Object obj)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd";
            return JsonConvert.SerializeObject(obj, timeFormat);
        }
        public static string GetJsObjNetSoft(Object obj, bool IsTime)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd  HH:mm:ss";
            return JsonConvert.SerializeObject(obj, timeFormat);
        }
       
      
      
        public static string GetTreeGridData(IList list,string idName,string pidName)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count == 0)
            {
                return "[]";
            }
            var obj = list[0];
            Type type = obj.GetType();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (pvalue.ToString() == "-1")
                {
                    sb.Append(BuildTreeGrid(list, type, idName, pidName, item) + ",");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Replace("\\","/");
            return "["+sb+"]";    
        }
        public static string GetTreeGridData(IList list, string idName, string pidName,string rootId)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count == 0)
            {
                return "[]";
            }
            var obj = list[0];
            Type type = obj.GetType();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (pvalue.ToString() == rootId)
                {
                    sb.Append(BuildTreeGrid(list, type, idName, pidName, item) + ",");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Replace("\\", "/");
            return "[" + sb + "]";
        }
      

        private static string BuildTreeGrid(IList list, Type type, string idName, string pidName, object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var item in type.GetProperties())
            {
                var name = item.Name;
                var value = item.GetValue(obj, null);
                //sb.Append("\"state\":\"closed\",");
                if (value != null)
                {
                    sb.Append("\"" + name + "\":\"" + value.ToString() + "\",");
                }
                else
                {
                    sb.Append("\"" + name + "\":\"\",");
                }
            }
            sb.Append("\"children\":[");
            var idValue = type.GetProperty(idName).GetValue(obj, null);
            StringBuilder sbChild = new StringBuilder();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (idValue.ToString() == pvalue.ToString())
                {
                    sbChild.Append(BuildTreeGrid(list, type, idName, pidName, item)+",");
                }
            }
            if (sbChild.Length > 0)
            {
                sbChild.Remove(sbChild.Length - 1, 1);
            }
            sb.Append(sbChild.ToString());
            sb.Append("]}");

            return sb.ToString();
        }
    
        public static string GetTreeData(IList list, string idName, string pidName, string textName)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count == 0)
            {
                return "[]";
            }
            var obj = list[0];
            Type type = obj.GetType();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (pvalue.ToString() == "-1")
                {
                    sb.Append(BuildTree(list, type, idName, pidName,textName, item) + ",");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Replace("\\", "/");
            return "[" + sb + "]";    
        }


        public static string GetTreeData(IList list, string idName, string pidName, string textName,string rootId)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count == 0)
            {
                return "[]";
            }
            var obj = list[0];
            Type type = obj.GetType();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (pvalue.ToString() == rootId)
                {
                    sb.Append(BuildTree(list, type, idName, pidName, textName, item) + ",");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Replace("\\", "/");
            return "[" + sb + "]";
        }

        public static string GetSimpleTreeData(IList list, string idName, string pidName, string textName)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count == 0)
            {
                return "[]";
            }
            var obj = list[0];
            Type type = obj.GetType();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (pvalue.ToString() == "-1")
                {
                    sb.Append(BuildSimpleTree(list, type, idName, pidName, textName, item) + ",");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Replace("\\", "/");
            return "[" + sb + "]";
        }


        // 树形数据
        private static string BuildTree(IList list, Type type, string idName, string pidName,string textName, object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"id\":\""+type.GetProperty(idName).GetValue(obj,null)+"\",");
            sb.Append("\"text\":\"" + type.GetProperty(textName).GetValue(obj, null) + "\",");
            sb.Append("\"attributes\":{");
            foreach (var item in type.GetProperties())
            {
                var name = item.Name;
                var value = item.GetValue(obj, null);
                if (value != null)
                {
                    sb.Append("\"" + name + "\":\"" + value.ToString() + "\",");
                }
                else
                {
                    sb.Append("\"" + name + "\":\"\",");
                }
            }
            sb.Remove(sb.Length-1,1);
            sb.Append("},");
            sb.Append("\"children\":[");
            var idValue = type.GetProperty(idName).GetValue(obj, null);
            StringBuilder sbChild = new StringBuilder();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (idValue.ToString() == pvalue.ToString())
                {
                    sbChild.Append(BuildTree(list, type, idName, pidName, textName,item) + ",");
                }
            }
            if (sbChild.Length > 0)
            {
                sbChild.Remove(sbChild.Length - 1, 1);
            }
            sb.Append(sbChild.ToString());
            sb.Append("]}");
            return sb.ToString();
        }

        private static string BuildSimpleTree(IList list, Type type, string idName, string pidName, string textName, object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"id\":\"" + type.GetProperty(idName).GetValue(obj, null) + "\",");
            sb.Append("\"text\":\"" + type.GetProperty(textName).GetValue(obj, null) + "\",");
           
            sb.Append("\"children\":[");
            var idValue = type.GetProperty(idName).GetValue(obj, null);
            StringBuilder sbChild = new StringBuilder();
            foreach (var item in list)
            {
                var pvalue = type.GetProperty(pidName).GetValue(item, null);
                if (idValue.ToString() == pvalue.ToString())
                {
                    sbChild.Append(BuildTree(list, type, idName, pidName, textName, item) + ",");
                }
            }
            if (sbChild.Length > 0)
            {
                sbChild.Remove(sbChild.Length - 1, 1);
            }
            sb.Append(sbChild.ToString());
            sb.Append("]}");
            return sb.ToString();
        }
       
        public static string GetServerIP()
        {
            string strAddr = "";
            string strHostName = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                foreach (IPAddress _ipaddress in ipEntry.AddressList)
                {
                    if (_ipaddress.AddressFamily.ToString().ToLower() == "internetwork")
                    {
                        strAddr = _ipaddress.ToString();
                        break;
                    }
                }
            return strAddr;
        }
        public static DateTime ConvertIntDateTime(long d)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(d);

            return dateTimeStart.Add(toNow);
        }
        public static string GetAreaName()
        {
            string area = HttpContext.Current.Request.RawUrl;
            if (area == "/")
            {
                return "";
            }
            area = area.Remove(0, 1);
            if (area.IndexOf("/") > 0)
            {
                area = area.Substring(0, area.IndexOf("/"));
            }
            else {
                area = "";
            }
           
            return area;
        }
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.PathAndQuery;
        }

      
    }
}