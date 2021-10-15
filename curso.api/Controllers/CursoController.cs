using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Models.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/cursos")]
    [ApiController]
    [Authorize] // Torna a rota privada, só pode ser acessada com autenticação
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoController(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        [SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar um novo curso.")]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado.")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno no servidor.")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
        {
            Curso curso = new Curso();
            curso.Nome = cursoViewModelInput.Nome;
            curso.Descricao = cursoViewModelInput.Descricao;
            var CodigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            curso.CodigoUsuario = CodigoUsuario;

            _cursoRepository.Adicionar(curso);
            _cursoRepository.Commit();
           
            return Created("", cursoViewModelInput);
        }

        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter os cursos.")]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado.")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno no servidor.")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            
            var cursos = _cursoRepository.ObterPorUsuario(codigoUsuario)
                .Select(s => new CursoViewModelOutput(){
                    Nome = s.Nome,
                    Descricao = s.Descricao,
                    Login = s.Usuario.Login
                });
            

            return Ok(cursos);
        }

    }
}
