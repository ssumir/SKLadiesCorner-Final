using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SKLadiesCorner.Models;

namespace SKLadiesCorner.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SKLadiesCorner.Models.Product> Product { get; set; } = default!;
        public DbSet<SKLadiesCorner.Models.Order> Order { get; set; } = default!;
        public DbSet<SKLadiesCorner.Models.OrderItem> OrderItem { get; set; } = default!;
        public DbSet<SKLadiesCorner.Models.Review> Review { get; set; } = default!;
        public DbSet<SKLadiesCorner.Models.ShoppingCart> ShoppingCart { get; set; } = default!;
        public DbSet<SKLadiesCorner.Models.User> User { get; set; } = default!;
        public DbSet<SKLadiesCorner.Models.Photo> Photo { get; set; } = default!;
        public DbSet<SKLadiesCorner.Models.OrderConfirm> OrderConfirm { get; set; } = default!;
    }
}
