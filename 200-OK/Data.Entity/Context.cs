using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _200_OK.Data.Entity
{
    public class ContextModel : DbContext
    {

        public ContextModel()
            : base("Database")
        {
            Database.SetInitializer<ContextModel>(new DropCreateDatabaseIfModelChanges<ContextModel>());
        }

        public DbSet<OperationData> Data { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Likes> Likes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
