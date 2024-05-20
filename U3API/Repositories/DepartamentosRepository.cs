using U3API.Models.Entities;

namespace U3API.Repositories
{
    public class DepartamentosRepository
    {
        public ItesrcneActividadesContext Context { get; }

        public DepartamentosRepository(ItesrcneActividadesContext context)
        {
            Context = context;
        }

        public IEnumerable<Departamentos> GetAll()
        {
            return Context.Departamentos.OrderBy(x => x.Id);
        }

        public void Insert(Departamentos dep)
        {
            Context.Departamentos.Add(dep);
            Context.SaveChanges();
        }

        public void Update(Departamentos dep)
        {
            Context.Departamentos.Update(dep);
            Context.SaveChanges();
        }

        public void Delete(Departamentos dep)
        {
            Context.Departamentos.Remove(dep);
            Context.SaveChanges();
        }
    }
}
