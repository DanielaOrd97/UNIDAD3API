using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ClienteWPF.Models.DTOs
{
    public class ActividadDTO
    {
        public int? Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? NombreDepto { get; set; }
        public DateOnly? FechaDeRealizacion { get; set; }
        public int? IdDepartamento { get; set; }
        public DateTime? FechaDeCreacion { get; set; }
        public string? Imagen { get; set; }
        public BitmapImage? Img { get; set; } = null!;
        public int Estado { get; set; }
        //public string? Error { get; set; }
    }
}
