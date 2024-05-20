using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.Models.DTOs
{
    public class ResponseDTO
    {
        public string? Token { get; set; }
        public bool? EsAdmin { get; set; }
        public string? Errores { get; set; }
    }
}
