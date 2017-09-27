using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Moni.Repository
{
    public abstract class GenericRepository<T> where T : class
    {
        public GenericRepository(MoniContext context)
        {
            Context = context;
        }

        private MoniContext Context { get; set; }

        public void Add(T obj)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Set<T>().Add(obj);
                    Context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Add(List<T> objList)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var obj in objList)
                    {
                        Context.Set<T>().Add(obj);
                        Context.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>().AsQueryable();
        }

        public class PaginatedDate
        {
            public PaginatedDate(IQueryable<T> query, int total)
            {
                Query = query;
                Total = total;
            }

            public IQueryable<T> Query { get; set; }
            public int Total { get; set; }
        }

        public virtual PaginatedDate GetAllWithPagination(int itensPerPage, int page)
        {
            var totalAsync = GetTotal();

            var query = Context.Set<T>().AsQueryable();

            // L2S doesn't like skipping 0 registers.
            if(page > 0)
            {
                query = query.Skip(itensPerPage * page);
            }

            totalAsync.Wait();

            return new PaginatedDate(query.Take(itensPerPage), totalAsync.Result);
        }

        private async Task<int> GetTotal()
        {
            // Uses dynamic quering to query count only for the Id and not for *.
            // WIP: EF Core does not have support for ad hoc mapping of arbitrary types.
            // This might work when EF Core gets updated.
            // Status: https://github.com/aspnet/EntityFrameworkCore/issues/1862
            return await Task.FromResult(GetAll().Select("new (Id)").Count());
        }

        public void Update(T obj)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Entry(obj).State = EntityState.Modified;
                    Context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Remove(T obj)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Set<T>().Remove(obj);
                    Context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
