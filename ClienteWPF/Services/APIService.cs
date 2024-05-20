using ClienteWPF.Models.DTOs;
using ClienteWPF.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClienteWPF.Services
{
    public class APIService
    {
        HttpClient cliente;
        public ActividadesRepository Rep;

        public APIService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("https://localhost:44326/api/")
            };

            Rep = new ActividadesRepository();
        }
        public static string? Token { get; set; }

        bool logeado = false;
        public async Task<ResponseDTO> LogIn(LogInDTO dto)
        {

            var response = await cliente.PostAsJsonAsync("login", dto);

            try
            {

                var respuesta = await response.Content.ReadAsStringAsync();

                var r = JsonConvert.DeserializeObject<ResponseDTO>(respuesta);

                ResponseDTO r1 = new()
                {
                    Token = r.Token,
                    EsAdmin = r.EsAdmin
                };

                Token = r1.Token;

                return r1;
            }
            catch (Exception)
            {
                var errores = await response.Content.ReadAsStringAsync();

                ResponseDTO resp = new()
                {
                    Token = null,
                    EsAdmin = false,
                    Errores = errores
                };

                return resp;
            }

        }

        public async Task<List<ActividadDTO>> GetActividades(string estado)
        {
            List<ActividadDTO> actividadeslista = new();


            if (string.IsNullOrEmpty(Token))
            {
                throw new InvalidOperationException("Usuario no autenticado.");
            }

            try
            {
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var response = await cliente.GetAsync($"actividades/{estado}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var actividades = JsonConvert.DeserializeObject<List<ActividadDTO>>(jsonResponse);

                if (actividades != null)
                {
                    foreach (ActividadDTO act in actividades)
                    {
                        //var entidad = Rep.Get(act.Id);
                        var entidad = Rep.Get((int)act.Id);

                        if (entidad != null)
                        {
                            ActividadDTO dto = new()
                            {
                                Id = act.Id,
                                Titulo = act.Titulo,
                                Descripcion = act.Descripcion,
                                IdDepartamento = act.IdDepartamento,
                                NombreDepto = act.NombreDepto,
                                FechaDeRealizacion = act.FechaDeRealizacion,
                                FechaDeCreacion = act.FechaDeCreacion,
                                Estado = act.Estado,
                                Imagen = act.Imagen
                            };


                            actividadeslista.Add(dto);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return actividadeslista;
        }
    }
}
