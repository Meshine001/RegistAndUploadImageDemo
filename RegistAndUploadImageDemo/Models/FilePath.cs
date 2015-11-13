using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistAndUploadImageDemo.Models
{
    public class FilePath
    {
        public int FilePathId { set; get; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        [StringLength(255)]
        public string BasePath { get; set; }
        public FileType FileType { get; set; }
        [Column(TypeName = "date")]
        public DateTime UploadTime { get; set; }
        public int TenUserID { get; set; }
        public virtual TenUser TenUser { get; set; }

    }
}