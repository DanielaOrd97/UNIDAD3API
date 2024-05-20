using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.Models.DTOs
{
    public class DepartamentoDTO
    {
        public int Id { get; set; }
        public string NombreDepartamento { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public int? IdSuperior { get; set; }
    }
}
