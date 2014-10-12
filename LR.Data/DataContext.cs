using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Configuration;

using LR.Models;
//using LR.Data.Configuration;

namespace LR.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public static string ConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConnectionStringName"] != null)
                {
                    return ConfigurationManager.AppSettings["ConnectionStringName"];
                }
                return "DefaultConnection";
            }
        }

        public DataContext() : base(nameOrConnectionString: DataContext.ConnectionStringName) { }
    }
}
