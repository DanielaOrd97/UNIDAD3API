using ClienteWPF.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClienteWPF.Services
{
    public class APIService
    {
        HttpClient cliente;

        public APIService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("https://localhost:44326/api/")
            };
        }

        public static string? Token { get; set; }

        bool logeado = false;
        public async Task<ResponseDTO> LogIn(LogInDTO dto)
        {
            
            var response = await cliente.PostAsJsonAsync("login", dto);

            try
            {

                //if (!response.IsSuccessStatusCode)
                //{
                //    var errores = await response.Content.ReadAsStringAsync();
                //    throw new Exception(errores);
                //}

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
    }
}
