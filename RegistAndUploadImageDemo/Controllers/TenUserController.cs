using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegistAndUploadImageDemo.Models;
using RegistAndUploadImageDemo.DAL;
using System.IO;

namespace RegistAndUploadImageDemo.Controllers
{
    public class TenUserController : Controller
    {
        private TenDBContext db = new TenDBContext();

        // GET: /TenUser/
        public ActionResult Index()
        {
            return View(db.TenUsers.ToList());
        }

        // GET: /TenUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
           TenUser tenuser = db.TenUsers.Include(u => u.FilePaths).SingleOrDefault(u => u.ID == id);
            
            if (tenuser == null)
            {
                return Json(new TenResult().Error("NotFound"), JsonRequestBehavior.AllowGet);
            }

            return Json(new TenResult().DetailsSuccess(tenuser), JsonRequestBehavior.AllowGet);
        }

        // GET: /TenUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TenUser/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,UserName,UserPassword,PhoneType,Gender,Birthday,JoinedDate,PCoin,OuterScore,InnerScore,Energy,Hobby,Quote,Lati,Longi")] TenUser tenuser,HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {

                    var avatar = new FilePath
                    {
                        FileName = tenuser.ID +"_"+ System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
                        BasePath = Path.Combine(Server.MapPath("~/images")),
                        FileType = FileType.Avatar,
                        UploadTime = DateTime.Now
                    };

                    upload.SaveAs(Path.Combine(avatar.BasePath, avatar.FileName));

                    tenuser.FilePaths = new List<FilePath>();
                    tenuser.FilePaths.Add(avatar);
                }
                db.TenUsers.Add(tenuser);
                db.SaveChanges();

                return Json(new TenResult().CreateSuccess(""+tenuser.ID));
            }

            return Json(new TenResult().Error("ParametersError"));
        }

        // GET: /TenUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TenUser tenuser = db.TenUsers.Include(u => u.FilePaths).SingleOrDefault(u => u.ID == id);
            if (tenuser == null)
            {
                return HttpNotFound();
            }
            return View(tenuser);
        }

        // POST: /TenUser/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        public ActionResult Edit([Bind(Include="ID,UserName,UserPassword,PhoneType,Gender,Birthday,JoinedDate,PCoin,OuterScore,InnerScore,Energy,Hobby,Quote,Lati,Longi")] TenUser tenuser)
        {
         
            if (ModelState.IsValid)
            {      
                db.Entry(tenuser).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new TenResult().Ok());
            }
            return Json(new TenResult().Error());
        }

        // GET: /TenUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TenUser tenuser = db.TenUsers.Find(id);
            if (tenuser == null)
            {
                return HttpNotFound();
            }
            return View(tenuser);
        }

        // POST: /TenUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TenUser tenuser = db.TenUsers.Find(id);
            db.TenUsers.Remove(tenuser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

         [HttpPost]
        public ActionResult UpdateAwatar(int? id, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                TenUser tenuser = db.TenUsers.Find(id);

                if (tenuser.FilePaths.Any(f => f.FileType == FileType.Avatar))
                {
                    string basepath = tenuser.FilePaths.First(f => f.FileType == FileType.Avatar).BasePath;
                    string filename = tenuser.FilePaths.First(f => f.FileType == FileType.Avatar).FileName;

                   
                    FilePath a = tenuser.FilePaths.First(f => f.FileType == FileType.Avatar);
                    db.FilePaths.Remove(a);
                    if (System.IO.File.Exists(Path.Combine(basepath, filename)))
                    {
                        System.IO.File.Delete(Path.Combine(basepath, filename));
                    }
                    tenuser.FilePaths.Remove(a);
                    var avatar = new FilePath
                    {
                        FileName = tenuser.ID + "_" + System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
                        BasePath = Path.Combine(Server.MapPath("~/images")),
                        FileType = FileType.Avatar,
                        UploadTime = DateTime.Now
                    };

                    upload.SaveAs(Path.Combine(avatar.BasePath, avatar.FileName));

                    tenuser.FilePaths.Add(avatar);

                    db.Entry(tenuser).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new TenResult().Ok());
                }


            }
            return Json(new TenResult().Error("NoUploadData"));
        }

        [HttpPost]
        public ActionResult UploadFiles(int? id, HttpPostedFileBase[] uploads)
        {
            if (uploads.Length != 0) 
            {

                TenUser tenuser = db.TenUsers.Find(id);
                foreach (HttpPostedFileBase upload in uploads)
                {
                    var photo = new FilePath
                    {
                        FileName = tenuser.ID + "_" + Guid.NewGuid().ToString()+System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
                        BasePath = Path.Combine(Server.MapPath("~/images")),
                        FileType = FileType.Photo,
                        UploadTime = DateTime.Now
                    };

                    upload.SaveAs(Path.Combine(photo.BasePath, photo.FileName));
                    tenuser.FilePaths.Add(photo);
                }

                db.Entry(tenuser).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new TenResult().Ok());

            }

            return Json(new TenResult().Error("NoUploadData"));
        }

        [HttpPost]
        public ActionResult DeleteFile(int? tenuserId, int? fileId) 
        {
            if (tenuserId == null || fileId == null)
            {
                return Json(new TenResult().Error("ParametersError"));
            }

            TenUser tenuser = db.TenUsers.Find(tenuserId);
            FilePath file = db.FilePaths.Find(fileId);
            string path = Path.Combine(file.BasePath,file.FileName);
            db.FilePaths.Remove(file);
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            tenuser.FilePaths.Remove(file);

            db.Entry(tenuser).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new TenResult().Ok());
            
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
