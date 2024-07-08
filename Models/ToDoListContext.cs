using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class ToDoListContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Priority> Priorities { get; set; }

        public string DbPath { get; }

        public ToDoListContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "to_do_list.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseLazyLoadingProxies()
            .UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Priority>()
                .HasKey(p => p.Level);
        }
    }
}
