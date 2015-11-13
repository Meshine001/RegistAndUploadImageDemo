using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RegistAndUploadImageDemo.Models;

namespace RegistAndUploadImageDemo.DAL
{
    public class TenDBContext:DbContext
    {
        public TenDBContext()
            : base("TenDbConnection")
        {

        }

        public DbSet<TenUser> TenUsers { get; set; }
        public DbSet<FilePath> FilePaths { get; set; }


    }
}