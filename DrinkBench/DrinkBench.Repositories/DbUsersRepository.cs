using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkBench.Models;

namespace DrinkBench.Repositories
{
    public class DbUsersRepository : IRepository<User>
    {
        private DbContext dbContext;
        private DbSet<User> entitySet;

        public DbUsersRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<User>();
        }

        public User Add(User item)
        {
            this.entitySet.Add(item);
            this.dbContext.SaveChanges();
            return item;
        }

        public User Update(int id, User item)
        {
            User dbUser = this.entitySet.Find(id);
            if (dbUser == null)
            {
                throw new ArgumentException("User not found!");
            }
            item.Id = id;
            dbUser = item;
            this.dbContext.SaveChanges();
            return dbUser;
        }

        public void Delete(int id)
        {
            User item = this.entitySet.Find(id);
            if (item == null)
            {
                throw new ArgumentException("User not found!");
            }
            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public void Delete(User item)
        {
            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public User Get(int id)
        {
            return this.entitySet.Find(id);
        }

        public IQueryable<User> All()
        {
            return this.entitySet;
        }

        public IQueryable<User> Find(System.Linq.Expressions.Expression<Func<User, int, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
