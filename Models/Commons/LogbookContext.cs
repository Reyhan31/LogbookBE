using Kalbe.Library.Common.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace LogBookAPI.Models
{
    public class LogbookContext : BaseDbContext<LogbookContext>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogbookContext(DbContextOptions<LogbookContext> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                // adjustmnet tambahan saat pembuatan db
                modelBuilder.Entity<LogbookItem>(e =>
                {
                    e.HasKey(x => x.Id);

                    e.HasOne(x => x.Logbook).WithMany(x => x.items);
                });

            }
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Logbook> Logbooks { get; set; }
        public virtual DbSet<LogbookItem> LogbookItems { get; set; }

    }
}