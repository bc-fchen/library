using CLMS.Entity;
using Microsoft.EntityFrameworkCore;

namespace CLMS.DAL
{
    /// <summary>
    /// 创建数据库上下文类
    /// </summary>
    public class DataContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<MenuEntity> Menus { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserRoleEntity> UserRoles { get; set; }

        public DbSet<RoleMenuEntity> RoleMenus { get; set; }

        /// <summary>
        /// 图书室
        /// </summary>
        public DbSet<LibraryEntity> Librarys { get; set; }

        /// <summary>
        /// 阅览架
        /// </summary>
        public DbSet<BookRackEntity> BookRacks { get; set; }

        /// <summary>
        /// 借还记录
        /// </summary>
        public DbSet<CirculateEntity> Circulates { get; set; }

        /// <summary>
        /// 图书
        /// </summary>
        public DbSet<BookEntity> Books { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<MenuEntity>().ToTable("Menus");
            modelBuilder.Entity<RoleEntity>().ToTable("Roles");
            modelBuilder.Entity<UserRoleEntity>().ToTable("UserRoles");
            modelBuilder.Entity<RoleMenuEntity>().ToTable("RoleMenus");
            //
            modelBuilder.Entity<LibraryEntity>().ToTable("Librarys");
            modelBuilder.Entity<BookRackEntity>().ToTable("BookRacks");
            modelBuilder.Entity<CirculateEntity>().ToTable("Circulates");
            modelBuilder.Entity<BookEntity>().ToTable("Books");
        }
    }
}