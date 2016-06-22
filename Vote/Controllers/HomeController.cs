using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVote;

namespace Vote.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Index(string vscode)
        {
            using (IDatabase bs = VoteDB.GetInstance())
            {
                VsCode code = bs.FirstOrDefault<VsCode>(Sql.Builder.Where("vscode=@0", vscode));
                if (code == null)
                {
                    ViewBag.mag = "alert('投票码输入错误!')";
                    return View();
                }
            }
            return RedirectToAction("list", new { vscode = vscode });
        }
        [HttpGet]
        public ActionResult list(string vscode)
        {
           
            using (IDatabase bs = VoteDB.GetInstance())
            {
                Vote_session vs = bs.FirstOrDefault<Vote_session>(Sql.Builder.Where("VState=1"));
                if (vs == null)
                {
                    ViewBag.mag = "alert('投票已经结束或尚未开始!')";
                    return RedirectToAction("voteorder");
                }
                List<UserInfo> list = bs.Fetch<UserInfo>(Sql.Builder.Where(" vsid= (select top 1 id from Vote_session where VState=1)"));
                List<Vote_User> vusers = bs.Fetch<Vote_User>(Sql.Builder.Where("vscode=@0 and VsID=(select top 1 id from Vote_session where VState=1)", vscode));
                if (vusers.Count>0)
                {
                    Sql sql = new Sql();
                    sql.Append(@"SELECT a.*,b.SumVote FROM UserInfo a
                 LEFT JOIN (SELECT UserID,SUM(SumVote) SumVote FROM (                
 SELECT b.UserID,CASE WHEN Vscode LIKE 'a%' THEN SumVote*3 ELSE SumVote END SumVote  FROM (                 
 SELECT UserID,Vscode, COUNT(*) SumVote FROM Vote_user GROUP BY UserID,Vscode) b) c GROUP BY c.UserID)b ON a.ID=b.UserID
                  WHERE  a.VSID=(select top 1 id from Vote_session where VState=1)");

                     list = bs.Fetch<UserInfo>(sql);
                     foreach (var item in list)
                     {
                         item.Numer = "-2";
                     }
                     foreach (var item in vusers)
                     {
                         list.Find(n => n.ID == item.UserID).UserDese = "(已投)";
                     }
                }
                else
                {
                    foreach (var item in list)
                    {
                        item.Numer = "-1";
                    }
                }
               
                ViewBag.list=list;
                ViewBag.info = vs;
                ViewBag.vscode = vscode;
            }
            return View();
        }
        [HttpPost]
        public ActionResult list(string vscode,int face,int back)
        {
            using (IDatabase bs = VoteDB.GetInstance())
            {
                Vote_session vs = bs.FirstOrDefault<Vote_session>(Sql.Builder.Append("select top 1 * from Vote_session where VState=1"));
                if (vs == null)
                {
                    ViewBag.mag = "alert('投票已经结束或尚未开始!')";
                    return RedirectToAction("voteorder");
                }
                VsCode code = bs.FirstOrDefault<VsCode>(Sql.Builder.Where("vscode=@0", vscode));
                if (code == null)
                {
                    ViewBag.mag = "alert('投票码输入错误!')";
                    return RedirectToAction("Index", new { vscode = vscode });
                }
                List<Vote_User> vusers = bs.Fetch<Vote_User>(Sql.Builder.Where("vscode=@0 and VsID=(select top 1 id from Vote_session where VState=1)", vscode));
                if (vusers.Count>0)
                { 

                    //ViewBag.mag = "alert('投票码已经投过当前场次!')";
                }
                else
                {
                    Vote_User vuserinfo = new Vote_User();
                    vuserinfo.UserID = face;
                    vuserinfo.CreateDate = DateTime.Now;
                    vuserinfo.Vscode = vscode;
                    vuserinfo.VsID = vs.ID;
                    bs.Insert<Vote_User>(vuserinfo);


                    Vote_User vuserinfo1 = new Vote_User();
                    vuserinfo1.UserID = back;
                    vuserinfo1.CreateDate = DateTime.Now;
                    vuserinfo1.Vscode = vscode;
                    vuserinfo1.VsID = vs.ID;
                    //投票
                    bs.Insert<Vote_User>(vuserinfo1);
                    vusers = bs.Fetch<Vote_User>(Sql.Builder.Where("vscode=@0 and VsID=(select top 1 id from Vote_session where VState=1)", vscode));
                   // return RedirectToAction("list", new { vscode=vscode });
                    ViewBag.mag = "alert('投票成功!')";
                }
               Sql sql=new Sql();
                sql.Append(@"SELECT a.*,b.SumVote FROM UserInfo a
                 LEFT JOIN (SELECT UserID,SUM(SumVote) SumVote FROM (                
 SELECT b.UserID,CASE WHEN Vscode LIKE 'a%' THEN SumVote*3 ELSE SumVote END SumVote  FROM (                 
 SELECT UserID,Vscode, COUNT(*) SumVote FROM Vote_user GROUP BY UserID,Vscode) b) c GROUP BY c.UserID)b ON a.ID=b.UserID
                  WHERE  a.VSID=(select top 1 id from Vote_session where VState=1)");
               
                List<UserInfo> list = bs.Fetch<UserInfo>(sql);
                foreach (var item in list)
                {
                    item.Numer = "-2";
                }
                foreach (var item in vusers)
                {
                    list.Find(n => n.ID == item.UserID).UserDese = "(已投)"; 
                }
                ViewBag.list = list;
                ViewBag.info = vs;
                ViewBag.vscode = vscode;
            }
            return View();
        }
        public ActionResult voteorder() {
            using (IDatabase bs = VoteDB.GetInstance())
            {
                Vote_session vs = bs.FirstOrDefault<Vote_session>(Sql.Builder.Append("select top 1 * from Vote_session where VState=1"));
                if (vs != null)
                {
                  
                    return RedirectToAction("index");
                }
                Sql sql = new Sql();
                sql.Append(@"SELECT a.*,b.SumVote FROM UserInfo a
                 LEFT JOIN (SELECT UserID,SUM(SumVote) SumVote FROM (                
 SELECT b.UserID,CASE WHEN Vscode LIKE 'a%' THEN SumVote*3 ELSE SumVote END SumVote  FROM (                 
 SELECT UserID,Vscode, COUNT(*) SumVote FROM Vote_user GROUP BY UserID,Vscode) b) c GROUP BY c.UserID)b ON a.ID=b.UserID
                  ");
                ViewBag.mag = "alert('投票已经结束或尚未开始!')";
                ViewBag.list = bs.Fetch<UserInfo>(sql).OrderByDescending(n=>n.SumVote).ToList();
            }
            return View();
        }

        public ActionResult VS()
        {
            using (IDatabase bs = VoteDB.GetInstance())
            {
                List<Vote_session> vs = bs.Fetch<Vote_session>();
                //ViewBag.list = vs;
                return View(vs);
            }
        }
        public ActionResult SetStart(int id)
        {
            try
            {
                using (IDatabase bs = VoteDB.GetInstance())
                {
                    bs.Update<Vote_session>(new Sql().Append("set VState=0 where VState=1"));
                    bs.Update<Vote_session>(new Sql().Append("set VState=1,StartDate=getDate(),endsDate=null where id=@0", id));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return RedirectToAction("vs");
        }
        public ActionResult SetEnd(int id)
        {
            try
            {
                using (IDatabase bs = VoteDB.GetInstance())
                {
                    bs.Update<Vote_session>(new Sql().Append("set VState=0,endsDate=getDate()  where id=@0", id));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return RedirectToAction("vs");
        }
        //public ActionResult SetEndAll()
        //{
        //    try
        //    {
        //        using (IDatabase bs = VoteDB.GetInstance())
        //        {
        //            bs.Update<Vote_session>(new Sql().Append("set VState=0,endsDate=getDate()  where id=@0", id));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //    return RedirectToAction("vs");
        //}
        public ActionResult Showinfo() {

            return View();
        }
        public ActionResult GetVsInfo(int vsid)
        {
             using (IDatabase bs = VoteDB.GetInstance())
             {
                 Vote_session vs = bs.FirstOrDefault<Vote_session>(Sql.Builder.Where("ID=@0",vsid));
                 Sql sql = new Sql();
                 sql.Append(@"SELECT a.*,b.SumVote FROM UserInfo a
                 LEFT JOIN (SELECT UserID,SUM(SumVote) SumVote FROM (                
 SELECT b.UserID,CASE WHEN Vscode LIKE 'a%' THEN SumVote*3 ELSE SumVote END SumVote  FROM (                 
 SELECT UserID,Vscode, COUNT(*) SumVote FROM Vote_user GROUP BY UserID,Vscode) b) c GROUP BY c.UserID)b ON a.ID=b.UserID
                  ").Where("a.vsid=@0",vsid);
                 List<UserInfo> listuser = bs.Fetch<UserInfo>(sql).OrderByDescending(n => n.SumVote).ToList();

                 int c = bs.Single<int>(Sql.Builder.Append("SELECT COUNT(*) CO FROM (SELECT Vscode,COUNT(*) aa FROM Vote_User WHERE VsID=@0 GROUP BY Vscode)t", vsid));
                 return Json(new { 
                     info = vs, 
                     arruser = listuser.Select(n => n.UserName), 
                     arrcount = listuser.Select(n => n.SumVote),
                     count = c
                 });
             }           
        }
        public ActionResult code()
        {
            using (IDatabase bs = VoteDB.GetInstance())
            {

                List<VsCode> listuser = bs.Fetch<VsCode>();
                return View(listuser);
            }
        }
    }
}
