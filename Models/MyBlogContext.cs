using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace razorweb.models{
    //razorweb.models.MyBlogContext
    public class MyBlogContext : IdentityDbContext<AppUser> {
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder buider) {
                    base.OnConfiguring(buider);
        }
         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entitytype in builder.Model.GetEntityTypes())
            {
                // entity type tương ứng với model trong cơ sỏ dữ liệu
                var tableName = entitytype.GetTableName();
                if(tableName.StartsWith("AspNet")){
                    entitytype.SetTableName(tableName.Substring(6));
                }
            }
            
        }
        public DbSet<Article> articles {set;get;}
            
    }
}