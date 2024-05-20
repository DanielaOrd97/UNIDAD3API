using ClienteWPF.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.Repositories
{
    public class Repository<T> where T : class
    {
        public ItesrcneActividadesContext Context { get; }
        public Repository(ItesrcneActividadesContext context)
        {
            Context = context;
        }

        public virtual T? Get(object id)
        {
            return Context.Find<T>(id);
        }

        public virtual void Insert(T entity)
        {
            Context.Add(entity);
        }

        public virtual void Update(T entity)
        {
            Context.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }
    }
}
