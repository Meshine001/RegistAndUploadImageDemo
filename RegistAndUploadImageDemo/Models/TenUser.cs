using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RegistAndUploadImageDemo.Models
{
    public class TenUser
    {
        public int ID { get; set; }

        [Required]
        [StringLength(32)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        public string UserPassword { get; set; }

        public byte PhoneType { get; set; }

        public byte Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }

        [Column(TypeName = "date")]
        public DateTime JoinedDate { get; set; }

        [Column(TypeName = "money")]
        public decimal PCoin { get; set; }

        public int OuterScore { get; set; }

        public int InnerScore { get; set; }

        public int Energy { get; set; }

        [StringLength(128)]
        public string Hobby { get; set; }

        [StringLength(128)] 
        public string Quote { get; set; }

        public double? Lati { get; set; }

        public double? Longi { get; set; }

        public virtual ICollection<FilePath> FilePaths { get; set; }

    }
}