using Microsoft.EntityFrameworkCore;

namespace razorweb.models{
    //razorweb.models.MyBlogContext
    public class MyBlogContext : DbContext {
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder buider) {
                    base.OnConfiguring(buider);
        }
         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
        public DbSet<Article> articles {set;get;}
            
    }
}