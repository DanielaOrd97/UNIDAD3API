using ClienteWPF.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.Repositories
{
    public class ActividadesRepository
    {
        private readonly ItesrcneActividadesContext context;

        public ActividadesRepository(ItesrcneActividadesContext c)
        {
            this.context = c;
            context = new();
        }

        public ActividadesRepository()
        {
            context = new();
        }

        public IEnumerable<Actividades> GetActEnBorrador()
        {
            return context.Actividades.Include(x => x.IdDepartamentoNavigation)
                .ThenInclude(x => x.IdSuperiorNavigation)
                .OrderBy(x => x.Id)
                .Where(x => x.Estado == 0);
        }

        public IEnumerable<Actividades> GetActPublicadas()
        {
            return context.Actividades.Include(x => x.IdDepartamentoNavigation)
                .ThenInclude(x => x.IdSuperiorNavigation)
                .OrderBy(x => x.Id)
                .Where(x => x.Estado == 1);
        }

        public IEnumerable<Actividades> GetActEliminadas()
        {
            return context.Actividades.Include(x => x.IdDepartamentoNavigation)
                .ThenInclude(x => x.IdSuperiorNavigation)
                .OrderBy(x => x.Id)
                .Where(x => x.Estado == 2);
        }

        public Actividades Get(object id)
        {
            return context.Find<Actividades>(id);
        }

        public void Update(Actividades act)
        {
            context.Update(act);
        }
    }
}
