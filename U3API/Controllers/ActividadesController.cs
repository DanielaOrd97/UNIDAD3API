using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U3API.Models.Validators;
using U3API.Models.DTOs;
using U3API.Models.Entities;
using U3API.Repositories;

namespace U3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesController : ControllerBase
    {
        public Repository<Actividades> Repo { get; }

        public ActividadesController(Repository<Actividades> repo)
        {
            Repo = repo;
        }

        /// <summary>
        /// AUTORIZACION DE USUARIOS MANUAL.
        /// </summary>

        /*
        [HttpGet("Direccion_General/{estado}")]
        [Authorize(Roles = "Director General")]
        public IActionResult GetAllActividades(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid, estado));
        }

        [HttpGet("Direccion_academica")]
        [Authorize(Roles = "Direccion Academica")]
        public IActionResult GetActividadesDireccionAcademica(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid,estado));
        }

        [HttpGet("Direccion_PlaneacionYVinculacion")]
        [Authorize(Roles = "DIRECCIÓN DE PLANEACIÓN Y VINCULACIÓN")]
        public IActionResult GetActividadesPlaneacionYVinculacion(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid, estado));
        }


        [HttpGet("Subireccion_academica")]
        [Authorize(Roles = "Subireccion Academica")]
        public IActionResult GetActividadesSubDireccionAcademica(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid,estado));

        }

        [HttpGet("Subireccion_PosgradoEInvestigacion")]
        [Authorize(Roles = "Subdirección De Posgrado E Investigación")]
        public IActionResult GetActividadesSubPosgradoEInv(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid, estado));

        }

        [HttpGet("Subireccion_DeVinculacion")]
        [Authorize(Roles = "Subdirección De Vinculación")]
        public IActionResult GetActividadesSubVinculacion(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid, estado));

        }

        [HttpGet("Subireccion_DePlaneacion")]
        [Authorize(Roles = "Subdirección De Planeación")]
        public IActionResult GetActividadesSubPlaneacion(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid, estado));

        }

        [HttpGet("Subireccion_Administrativa")]
        [Authorize(Roles = "Subdirección Administrativa")]
        public IActionResult GetActividadesSubAdmin(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividades(deptid, estado));

        }

        */

        /// <summary>
        /// METODO GET PARA TODAS LOS DEPARTAMENTOS REGISTRADOS DE ACUERDO AL TOKEN.
        /// </summary>

        //[HttpGet("{division}/{estado}")]
        [HttpGet("{estado}")]
        [Authorize] 
        public IActionResult GetActividadesDivisiones(string estado)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            return Ok(GetActividadesEstado(deptid, estado));
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllActividades()
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            var actividades = Repo.GetAllActWithInclude()
    .Where(x => (x.Estado == 0 || x.Estado == 1) && (x.IdDepartamento == deptid || x.IdDepartamentoNavigation.IdSuperior == deptid))
                                .OrderBy(x => x.FechaActualizacion).Select(x => new ActividadDTO
                                {
                                    Id = x.Id,
                                    Titulo = x.Titulo,
                                    Descripcion = x.Descripcion ?? "",
                                    IdDepartamento = x.IdDepartamento,
                                    NombreDepto = x.IdDepartamentoNavigation.Nombre,
                                    FechaDeRealizacion = x.FechaRealizacion,
                                    FechaDeCreacion = x.FechaCreacion,
                                    Estado = x.Estado,
                                    Imagen = GetImagenBase64(x.Id)
                                });



            return Ok(actividades);
        }

        /// <summary>
        /// FILTROS ACTIVIDADES DE ACUERDO A ESTADO Y AL ID DEL DEPARTAMENTO.
        /// </summary>
        /// 

        private string GetImagenBase64(int id)
        {
            string directorio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", $"{id}.png");

            if (System.IO.File.Exists(directorio))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(directorio);
                return Convert.ToBase64String(bytes);
            }
            else
            {
                return null;
            }
        }
        private IEnumerable<ActividadDTO> GetActividadesEstado(int deptId, string estado)
        {

            estado = estado.ToLower();

            if (estado == "borrador")
            {
                var actividades = Repo.GetActEnBorrador()
                               .Where(x => x.IdDepartamento == deptId || x.IdDepartamentoNavigation.IdSuperior == deptId)
                               .OrderBy(x => x.FechaRealizacion).Select(x => new ActividadDTO
                               {
                                   Id = x.Id,
                                   Titulo = x.Titulo,
                                   Descripcion = x.Descripcion ?? "",
                                   IdDepartamento = x.IdDepartamento,
                                   NombreDepto = x.IdDepartamentoNavigation.Nombre,
                                   FechaDeRealizacion = x.FechaRealizacion,
                                   FechaDeCreacion = x.FechaCreacion,
                                   Estado = x.Estado,
                                   Imagen = GetImagenBase64(x.Id)
                               }).ToList();

                return actividades;
            }
            else if (estado == "publicadas")
            {
                var actividades = Repo.GetActPublicadas()
                               .Where(x => x.IdDepartamento == deptId || x.IdDepartamentoNavigation.IdSuperior == deptId)
                               .OrderBy(x => x.FechaRealizacion).Select(x => new ActividadDTO
                               {
                                   Id = x.Id,
                                   Titulo = x.Titulo,
                                   Descripcion = x.Descripcion ?? "",
                                   IdDepartamento = x.IdDepartamento,
                                   NombreDepto = x.IdDepartamentoNavigation.Nombre,
                                   FechaDeRealizacion = x.FechaRealizacion,
                                   FechaDeCreacion = x.FechaCreacion,
                                   Estado = x.Estado,
                                   Imagen = GetImagenBase64(x.Id)
                               }).ToList();

                return actividades;
            }
            else if (estado == "eliminadas")
            {
                var actividades = Repo.GetActEliminadas()
                               .Where(x => x.IdDepartamento == deptId || x.IdDepartamentoNavigation.IdSuperior == deptId)
                               .OrderBy(x => x.FechaRealizacion).Select(x => new ActividadDTO
                               {
                                   Id = x.Id,
                                   Titulo = x.Titulo,
                                   Descripcion = x.Descripcion ?? "",
                                   IdDepartamento = x.IdDepartamento,
                                   NombreDepto = x.IdDepartamentoNavigation.Nombre,
                                   FechaDeRealizacion = x.FechaRealizacion,
                                   FechaDeCreacion = x.FechaCreacion,
                                   Estado = x.Estado,
                                   Imagen = GetImagenBase64(x.Id)
                               }).ToList();

                return actividades;
            }
            //return BadRequest("Estado de actividad no válido, solo pueden ser: Borrador, Publicadas o Eliminadas");
            return null;
        }


        [HttpPost("AgregarActividad")]
        [Authorize]
        public IActionResult PostAct(ActividadDTO dto)
        {

            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");


            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);

            int.TryParse(deptIdClaim.Value, out int deptid);

            if (resultados.IsValid)
            {
                Actividades entidad = new()
                {
                    Id = 0,
                    Titulo = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    FechaRealizacion = dto.FechaDeRealizacion,
                    IdDepartamento = deptid,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    Estado = dto.Estado
                };

               

                if (entidad.IdDepartamento == deptid)
                {
                    Repo.Insert(entidad);

                    string directorio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", $"{entidad.Id}.png");
                    byte[] bytes = Convert.FromBase64String(dto.Imagen);
                    System.IO.File.WriteAllBytes(directorio, bytes);

                    return Ok();
                }

                return BadRequest("Debes agregar la actividad deseada a tu departamento correspondiente.");
            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        }


        [HttpPut("EditarActividad/{id}")]
        [Authorize] 
        public IActionResult PutAct(ActividadDTO dto)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                var actividad = Repo.Get(dto.Id ?? 0);

                if (actividad == null || actividad.Estado == 2)
                {
                    return NotFound();
                }
                else
                {
                    //actividad.Titulo = dto.Titulo;
                    //actividad.Descripcion = dto.Descripcion;
                    //actividad.FechaRealizacion = dto.FechaDeRealizacion;
                    //actividad.IdDepartamento = (int)dto.IdDepartamento;
                    ////actividad.FechaCreacion = (DateTime)dto.FechaDeCreacion;
                    //actividad.FechaActualizacion = DateTime.Now;
                    //actividad.Estado = dto.Estado;


                    int.TryParse(deptIdClaim.Value, out int deptid);

                    if (actividad.IdDepartamento == deptid)
                    {
                        actividad.Titulo = dto.Titulo;
                        actividad.Descripcion = dto.Descripcion;
                        actividad.FechaRealizacion = dto.FechaDeRealizacion;
                       // actividad.IdDepartamento = (int)dto.IdDepartamento;
                        //actividad.FechaCreacion = (DateTime)dto.FechaDeCreacion;
                        actividad.FechaActualizacion = DateTime.Now;
                        actividad.Estado = dto.Estado;

                        Repo.Update(actividad);
                        return Ok();
                    }

                    return BadRequest("El departamento no es el correspondiente");
                }

            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        }

        [HttpDelete("EliminarActividad/{id}")]
        [Authorize]
        public IActionResult DeleteAct(int id)
        {
            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            var entidadAct = Repo.Get(id);

            if (entidadAct == null || entidadAct.Estado == 2)
            {
                return NotFound();
            }

            int.TryParse(deptIdClaim.Value, out int deptid);

            if(entidadAct.IdDepartamento == deptid)
            {
                entidadAct.Estado = 2;
                entidadAct.FechaActualizacion = DateTime.Now;
                Repo.Update(entidadAct);
                return Ok();
            }

            return BadRequest("La actividad que desea eliminar no es correspondiente a su departamento.");
            
        }
    }
}



        //[HttpPut("{id}")]
        //public IActionResult PutAct(ActividadDTO dto)
        //{
        //    ActividadValidator validator = new();
        //    var resultados = validator.Validate(dto);

        //    if (resultados.IsValid)
        //    {
        //        var actividad = Repo.Get(dto.Id ?? 0);

        //        if (actividad == null || actividad.Estado == 3)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            actividad.Titulo = dto.Titulo;
        //            actividad.Descripcion = dto.Descripcion;
        //            actividad.FechaRealizacion = dto.FechaDeRealizacion;
        //            actividad.IdDepartamento = dto.IdDepartamento;
        //            actividad.FechaCreacion = dto.FechaDeCreacion;
        //            actividad.FechaActualizacion = DateTime.UtcNow;
        //            actividad.Estado = dto.Estado;  

        //            Repo.Update(actividad);

        //            return Ok();
        //        }
        //    }

        //    return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        //}


        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var actividad = Repo.Get(id);

        //    if(actividad == null || actividad.Estado == 3)
        //    {
        //        return NotFound();
        //    }

        //    actividad.Estado = 3;
        //    actividad.FechaActualizacion = DateTime.UtcNow;
        //    Repo.Update(actividad);
        //    return Ok();
        //}
    //}
//}
