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
        public DbSet<Friend> Friends { get; set; }

        public DrinkBenchContext()
            : base("DrinkBenchDb")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasMany(X => X.FriendsId).WithMany(x => x.FriendsId).Map(map =>
        //            {
        //                map.ToTable("UsersFriends");
        //                map.MapLeftKey("UserId");
        //                map.MapRightKey("FriendId");
        //            });
        //        base.OnModelCreating(modelBuilder);
        //}
    }
}
