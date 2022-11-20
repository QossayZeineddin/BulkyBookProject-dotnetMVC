using BulkyBook.Areas.Admin.Models;
using BulkyBook.Areas.Customer.Models;
using BulkyBook.PublicModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Data 
{
    public class ApplecationDbContext : IdentityDbContext
    {
        public ApplecationDbContext(DbContextOptions<ApplecationDbContext> options) : base(options)
        {
        }

        /*   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder1)
           {
               optionsBuilder1
                   .UseMySQL("server=127.0.0.1;port=3306;user=root;password=root123;darabase=bulky")              
   
           }*/
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Categery> categeries { get; set; }
        public DbSet<ApplecationUser> applecationUsers { get; set; } 
        public DbSet<Company> companies { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        
        // protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        // {
        //     optionBuilder.UseMySQL("server=localhost;database=testing;uid=root;pwd=root123;");
        // }
    }
}