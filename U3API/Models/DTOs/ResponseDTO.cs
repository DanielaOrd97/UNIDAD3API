namespace U3API.Models.DTOs
{
    public class ResponseDTO
    {
        public string? Token { get; set; }
        public IEnumerable<AsociadoDTO>? ListaAsociados { get; set; }
    }

    public class AsociadoDTO
    {
        public int Id { get; set; }
        public string? NombreAsociado { get; set; }
    }
}
