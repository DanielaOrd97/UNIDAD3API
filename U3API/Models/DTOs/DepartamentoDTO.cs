namespace U3API.Models.DTOs
{
    public class DepartamentoDTO
    {
        public int? Id { get; set; }
        public string NombreDepartamento { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? IdSuperior { get; set; }
    }
}
