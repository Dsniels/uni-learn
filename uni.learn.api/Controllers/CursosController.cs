using System.Security.AccessControl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{

    public class CursosController : ControllerBase
    {
        private readonly IGenericRepository<Curso> _cursoRepository;

        public CursosController(IGenericRepository<Curso> cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCursos()
        {
            var cursos = await _cursoRepository.GetAllAsync();

            return Ok(cursos);
        }


        [HttpPost("Add")]
        public async Task<ActionResult> AddCurso(Curso curso)
        {
            var result = await _cursoRepository.Add(curso);

            if (result == 0)
            {
                return BadRequest();
            }

            return Ok(curso);
        }
    }
}
