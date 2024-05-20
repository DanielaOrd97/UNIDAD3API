using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using U3API.Helpers;
using U3API.Models.DTOs;
using U3API.Models.Entities;
using U3API.Models.Validators;
using U3API.Repositories;

namespace U3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        public Repository<Departamentos> Repo { get; }
        public JwtTokenGenerator JwtGenerator {  get; } 

        public LogInController(Repository<Departamentos> repo, JwtTokenGenerator jwtGenerator)
        {
            Repo = repo;
            JwtGenerator = jwtGenerator;
        }


        //[HttpPost]
        //public IActionResult LogIn(LogInDto dto)
        //{

        //    LogInValidator validator = new();
        //    var resultados = validator.Validate(dto);

        //    if (resultados.IsValid)
        //    {
        //        var usuario = Authenticate(dto);

        //        if (usuario != null)
        //        {
        //            dto.NombreDept = usuario.Nombre;
        //            JwtTokenGenerator jwtToken = new();
        //            return Ok(jwtToken.GetToken(dto));
        //        }

        //        return NotFound("Acceso denegado");
        //    }

        //    return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));

        //}

        //private Departamentos Authenticate(LogInDto dto)
        //{
        //    var pass = ConvertPasswordToSHA512(dto.Password);
        //    var usuario = Repo.GetAll().Where(x => x.Username.ToLower() == dto.Username.ToLower() && x.Password == pass).FirstOrDefault();

        //    if (usuario != null)
        //    {
        //        return usuario;
        //    }

        //    return null;
        //}

        [HttpPost]
        public IActionResult LogIn(LogInDto dto)
        {
            LogInValidator validator = new();
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                var usuario = Authenticate(dto);

                if (usuario != null)
                {
                    var token = JwtGenerator.GetToken(usuario.Username, usuario.Nombre, new List<Claim> { new Claim("id", usuario.Id.ToString()) });
                    var user = Repo.Get(usuario.Id);
                    bool admin = false;

                    if (user != null && user.IdSuperior == null)
                    {
                        admin = true;
                    }

                    //var asociados = Repo.GetActAsociadas(usuario.Id);

                    //ResponseDTO response = new ResponseDTO
                    //{
                    //    Token = token,
                    //    ListaAsociados = asociados
                    //};


                    LogInResponseDTO response = new()
                    {
                        Token = token,
                        EsAdmin = admin
                    };

                    return Ok(response);


                    //return Ok(token);
                    //var asociados = Repo.GetActAsociadas(usuario.Id);

                    //ResponseDTO response = new ResponseDTO
                    //{
                    //    Token = token,
                    //    ListaAsociados = asociados
                    //};

                    //return Ok(response);
                }

                return Unauthorized("Acceso denegado");
            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));

        }

        private Departamentos Authenticate(LogInDto dto)
        {
            var pass = ConvertPasswordToSHA512(dto.Password);
            var usuario = Repo.GetAll().Where(x => x.Username.ToLower() == dto.Username.ToLower() && x.Password == pass).FirstOrDefault();


            if (usuario != null)
            {
                return usuario;
            }

            return null;
        }



        public static string ConvertPasswordToSHA512(string password)
        {
            using (var sha512 = SHA512.Create())
            {
                var arreglo = Encoding.UTF8.GetBytes(password);
                var hash = sha512.ComputeHash(arreglo);
                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
