﻿using BulkyBook.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Areas.Admin.Data
{
    public class ApplecationDbContext : DbContext
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
        // protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        // {
        //     optionBuilder.UseMySQL("server=localhost;database=testing;uid=root;pwd=root123;");
        // }
    }
}