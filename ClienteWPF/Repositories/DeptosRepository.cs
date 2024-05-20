using ClienteWPF.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.Repositories
{
    public class DeptosRepository
    {
        private readonly ItesrcneActividadesContext context;

        public DeptosRepository(ItesrcneActividadesContext c)
        {
            this.context = c;
            context = new();
        }

        public DeptosRepository()
        {
            context = new();
        }

        public Departamentos Get(object id)
        {
            return context.Find<Departamentos>(id);
        }


        public IEnumerable<Departamentos> GetAll()
        {
            return context.Departamentos.OrderBy(x => x.Id);
        }
    }
}
