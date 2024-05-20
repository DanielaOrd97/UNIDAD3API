using U3API.Models.Entities;

namespace U3API.Models.DTOs
{
    public class ActividadDTO
    {
        public int? Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? NombreDepto { get; set; }
        public DateOnly? FechaDeRealizacion { get; set; }
        public int? IdDepartamento { get; set; }
        public string? Imagen { get; set; }
        public DateTime? FechaDeCreacion { get; set; }
        //public DateTime FechaDeActualizacion { get; set; }
        public int Estado { get; set; }

    }
}
