using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkBench.Models;

namespace DrinkBench.Repositories
{
    public class DbBenchesRepository : IRepository<Bench>
    {
        private DbContext dbContext;
        private DbSet<Bench> entitySet;

        public DbBenchesRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<Bench>();
        }

        public Bench Add(Bench item)
        {
            this.entitySet.Add(item);
            this.dbContext.SaveChanges();
            return item;
        }

        public Bench Update(int id, Bench item)
        {
            Bench dbItem = this.entitySet.Find(id);
            if (dbItem==null)
            {
                throw new ArgumentException("Bench not found!");
            }
            item.Id = id;
            dbItem = item;
            this.dbContext.SaveChanges();
            return dbItem;
        }

        public void Delete(int id)
        {
            Bench item = this.entitySet.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Bench not found!");
            }
            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public void Delete(Bench item)
        {
            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public Bench Get(int id)
        {
            return this.entitySet.Find(id);
        }

        public IQueryable<Bench> All()
        {
            return this.entitySet;
        }

        public IQueryable<Bench> Find(System.Linq.Expressions.Expression<Func<Bench, int, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
