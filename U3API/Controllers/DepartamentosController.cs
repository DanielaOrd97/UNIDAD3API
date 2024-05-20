using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U3API.Models.DTOs;
using U3API.Models.Entities;
using U3API.Models.Validators;
using U3API.Repositories;

namespace U3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartamentosController : ControllerBase
    {
        public Repository<Departamentos> Repository { get; }

        public DepartamentosController(Repository<Departamentos> repository)
        {
            Repository = repository;
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetAllDepartamentos()
        {

            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            var departamento = Repository.Get(deptid);

            if(departamento.IdSuperior == null)
            {
                var deptos = Repository.GetAll().
                          OrderBy(x => x.Id).
                          Select(x => new DepartamentoDTO
                          {
                              Id = x.Id,
                              NombreDepartamento = x.Nombre,
                              Username = x.Username,
                              Password = x.Password,
                              IdSuperior = x.IdSuperior
                          });
                return Ok(deptos);
            }

            return Unauthorized("No eres administrador");
          
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(DepartamentoDTO dto)
        {

            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            var departamento = Repository.Get(deptid);


            if (departamento.IdSuperior == null)
            {
                DepartamentoValidator validator = new();

                var resultados = validator.Validate(dto);

                if (resultados.IsValid)
                {
                    Departamentos entity = new()
                    {
                        Id = 0,
                        Nombre = dto.NombreDepartamento,
                        Username = dto.Username,
                        Password = dto.Password,
                        IdSuperior = dto.IdSuperior
                    };

                    Repository.Insert(entity);
                    return Ok();
                }

                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
            }

            return Unauthorized("No eres administrador");
        }


        [HttpPut("{id}")]
        public IActionResult Put(DepartamentoDTO dto)
        {

            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            var departamento = Repository.Get(deptid);

            if (departamento.IdSuperior == null)
            {
                DepartamentoValidator validator = new();

                var resultados = validator.Validate(dto);

                if (resultados.IsValid)
                {
                    var entidadDept = Repository.Get(dto.Id);

                    if (entidadDept == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        entidadDept.Nombre = dto.NombreDepartamento;
                        entidadDept.Username = dto.Username;
                        entidadDept.Password = dto.Password;
                        entidadDept.IdSuperior = dto.IdSuperior;

                        Repository.Update(entidadDept);

                        return Ok();
                    }
                }

                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));

            }

            return Unauthorized("No eres administrador");
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entidadDept = Repository.Get(id);

            if (entidadDept == null)
            {
                return NotFound();
            }


            var deptIdClaim = User.Claims.FirstOrDefault(x => x.Type == "id");

            if (deptIdClaim == null || !int.TryParse(deptIdClaim.Value, out int deptid))
            {
                return Unauthorized();
            }

            var departamento = Repository.Get(deptid);

            if (departamento.IdSuperior == null)
            {
                Repository.Delete(entidadDept);

                return Ok();
            }

            return Unauthorized("No eres administrador");

        }
    }
}
