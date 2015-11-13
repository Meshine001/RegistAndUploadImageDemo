using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistAndUploadImageDemo.DAL;
using System.IO;

namespace RegistAndUploadImageDemo.Controllers
{
    public class FileController : Controller
    {
        private TenDBContext db = new TenDBContext();
        //
        // GET: /File/
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.FilePaths.Find(id);
            FileStream fileStream = new FileStream(Path.Combine(Server.MapPath("~/images"), fileToRetrieve.FileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return File(fileStream,fileToRetrieve.ContentType);
        }
	}
}