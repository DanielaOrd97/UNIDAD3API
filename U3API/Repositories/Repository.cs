using Microsoft.EntityFrameworkCore;
using U3API.Models.DTOs;
using U3API.Models.Entities;

namespace U3API.Repositories
{
    public class Repository <T> where T : class
    {
        public ItesrcneActividadesContext Context { get; }

        public Repository(ItesrcneActividadesContext context)
        {
            Context = context;
        }

        public IEnumerable<Actividades> GetAllActWithInclude()
        {
            return Context.Actividades
                .Include(x => x.IdDepartamentoNavigation).ThenInclude(x => x.IdSuperiorNavigation)
                .OrderBy(x => x.Id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public IEnumerable<Actividades> GetActEnBorrador()
        {
            return Context.Actividades.Include(x => x.IdDepartamentoNavigation)
                .ThenInclude(x => x.IdSuperiorNavigation)
                .OrderBy(x => x.FechaActualizacion)
                .Where(x => x.Estado == 0);
        }

        public IEnumerable<Actividades> GetActPublicadas()
        {
            return Context.Actividades.Include(x => x.IdDepartamentoNavigation)
                .ThenInclude(x => x.IdSuperiorNavigation)
                .OrderBy(x => x.FechaActualizacion)
                .Where(x => x.Estado == 1);
        }

        public IEnumerable<Actividades> GetActEliminadas()
        {
            return Context.Actividades.Include(x => x.IdDepartamentoNavigation)
                .ThenInclude(x => x.IdSuperiorNavigation)
                .OrderBy(x => x.FechaActualizacion)
                .Where(x => x.Estado == 2);
        }

        public IEnumerable<AsociadoDTO> GetActAsociadas(int id)
        {
            return Context.Departamentos.Where(x => x.IdSuperior == id || x.Id == id)
                .Select(x => new AsociadoDTO
                {
                    Id = x.Id,
                    NombreAsociado = x.Nombre
                }).ToList();
        }

        public virtual T? Get(object id)
        {
            return Context.Find<T>(id);
        }

        public virtual void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {

            Context.Remove(entity);
            Context.SaveChanges();
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
