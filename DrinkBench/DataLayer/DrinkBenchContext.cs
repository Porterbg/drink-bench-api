using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkBench.Models;

namespace DataLayer
{
    public class DrinkBenchContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bench> Benches { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DrinkBenchContext()
            : base("DrinkBenchDb")
        {
        }
    }
}
