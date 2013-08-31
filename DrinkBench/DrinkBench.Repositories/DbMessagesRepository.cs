using DrinkBench.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkBench.Repositories
{
    public class DbMessagesRepository : IRepository<Message>
    {
        private DbContext dbContext;
        private DbSet<Message> entitySet;

        public DbMessagesRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<Message>();
        }

        public Message Add(Message item)
        {
            this.entitySet.Add(item);
            this.dbContext.SaveChanges();
            return item;
        }

        public Message Update(int id, Message item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            Message item = this.entitySet.Find(id);
            if (item == null)
            {
                throw new ArgumentException("User not found!");
            }
            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public void Delete(Message item)
        {
            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public Message Get(int id)
        {
            return this.entitySet.Find(id);
        }

        public IQueryable<Message> All()
        {
            return this.entitySet;
        }

        public IQueryable<Message> Find(System.Linq.Expressions.Expression<Func<Message, int, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
