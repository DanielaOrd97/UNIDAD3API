using ClienteWPF.Models.DTOs;
using ClienteWPF.Repositories;
using ClienteWPF.ViewModels;
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
        public int Est { get; set; }
        public string EstadoText { get; set; }


        public APIService()
        {
            cliente = new()
            {
                 BaseAddress = new Uri("https://localhost:44326/api/")
                //BaseAddress = new Uri("https://actividadesequipo8.websitos256.com/api/")
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

        public async Task<List<ActividadDTO>> GetAllActividades()
        {
            List<ActividadDTO> actividadeslista = new();


            if (string.IsNullOrEmpty(Token))
            {
                throw new InvalidOperationException("Usuario no autenticado.");
            }

            try
            {
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var response = await cliente.GetAsync($"actividades");
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
            catch (Exception)
            {

            }

            return actividadeslista;
        }

        public async Task AgregarActividad(ActividadDTO dto)
        {
            //if (string.IsNullOrEmpty(Token))
            //{
            //    throw new InvalidOperationException("Usuario no autenticado.");
            //}
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            try
            {
                var response = await cliente.PostAsJsonAsync("actividades/AgregarActividad", dto);


                if (response.IsSuccessStatusCode)
                {
                    Est = dto.Estado;

                    switch (Est)
                    {
                        case 0:
                            EstadoText = "Borrador";
                            break;
                        case 1:
                            EstadoText = "Publicadas";
                            break;
                        case 2:
                            EstadoText = "Eliminadas";
                            break;
                    }

                    await GetActividades(EstadoText);
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Usuario no autenticado.");

            }
        }

        public async Task ActualizarActividad(ActividadDTO dto)
        {
            if (dto != null)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    throw new InvalidOperationException("Usuario no autenticado.");
                }

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                try
                {
                    var response = await cliente.PutAsJsonAsync($"actividades/EditarActividad/{dto.Id}", dto);

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var entidad = Rep.Get((int)dto.Id);

                        if (entidad != null)
                        {
                            if (dto.Titulo != entidad.Titulo || dto.Descripcion != entidad.Descripcion || dto.FechaDeRealizacion != entidad.FechaRealizacion)
                            {

                                if(dto.Estado != entidad.Estado)
                                {
                                    Est = entidad.Estado;

                                    switch (Est)
                                    {
                                        case 0:
                                            EstadoText = "Borrador";
                                            break;
                                        case 1:
                                            EstadoText = "Publicadas";
                                            break;
                                        case 2:
                                            EstadoText = "Eliminadas";
                                            break;
                                    }

                                    await GetActividades(EstadoText);
                                }

                                Rep.Update(entidad);

                                Est = dto.Estado;

                                switch (Est)
                                {
                                    case 0:
                                        EstadoText = "Borrador";
                                        break;
                                    case 1:
                                        EstadoText = "Publicadas";
                                        break;
                                    case 2:
                                        EstadoText = "Eliminadas";
                                        break;
                                }

                                await GetActividades(EstadoText);

                                //corregir actualizacion de lista. 
                            }

                        }
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        public async Task EliminarActividad(int idAct)
        {
            if(idAct != 0)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    throw new InvalidOperationException("Usuario no autenticado.");
                }

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                try
                {
                    var response = await cliente.DeleteAsync($"actividades/EliminarActividad/{idAct}");

                    if (response.IsSuccessStatusCode)
                    {
                        var act = Rep.Get((int)idAct);

                        Est = act.Estado;

                        switch (Est)
                        {
                            case 0:
                                EstadoText = "Borrador";
                                break;
                            case 1:
                                EstadoText = "Publicadas";
                                break;
                        }

                        await GetActividades(EstadoText);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
