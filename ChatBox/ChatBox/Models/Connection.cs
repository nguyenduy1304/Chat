using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ChatBox.Models
{
    public partial class Connection : DbContext
    {
        public Connection()
            : base("name=Connection")
        {
        }

        public virtual DbSet<ZaloUser> ZaloUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
