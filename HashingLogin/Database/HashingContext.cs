using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashingLogin.Database
{
    public class HashingContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=SIMONNGF2DATA\\H2SQLSOMMERSIMON;Database=hashingLogin; Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<loginInformation> LoginInformation { get; set; }

    }
}
