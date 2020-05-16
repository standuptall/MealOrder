using BillMealMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BillMealMVC.Model
{
    public class MealContext : DbContext
    {
        public MealContext() : base("MealContext")
        {
            Database.SetInitializer<MealContext>(null);
        }
        public DbSet<CartHead> Cart { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<MenuHead> MenuHeads { get; set; }
        public DbSet<MenuRow> MenuRows { get; set; }
    }
}