using System.Security.AccessControl;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualBasic;
using uni.learn.api.EntitiesDto;
using uni.learn.api.Extensions;
using uni.learn.core.Entities;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{

    public class CursosController : BaseController
    {
        private readonly IGenericRepository<Curso> _genericRepository;
        private readonly UserManager<Usuario> _userManager;
        private readonly IGenericRepository<Temas> _temasRepository;
        private readonly ICursoRepository _cursosRepository;
        private readonly IMapper _mapper;

        public CursosController(
            IMapper mapper, 
            UserManager<Usuario> userManager, 
            IGenericRepository<Temas> temasRepository, 
            IGenericRepository<Curso> repository,
            ICursoRepository cursoRepository
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _temasRepository = temasRepository;
            _cursosRepository = cursoRepository;
            _genericRepository = repository;
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCursos()
        {
            var cursos = await _cursosRepository.GetAll();

            return Ok(cursos);
        }

        [HttpGet("GetApprovedCursos")]
        public async Task<IActionResult> GetApprovedCursos()
        {
            var cursos = await _cursosRepository.GetApprovedCursos();
            return Ok(cursos);
        }


        [HttpGet("GetUnApprovedCursos")]
        public async Task<IActionResult> GetUnApprovedCursos()
        {
            var cursos = await _cursosRepository.GetUnApprovedCursos();
            return Ok(cursos);

        }





        [Authorize]
        [HttpGet("GetCursosVistos")]
        public async Task<IActionResult> GetVistosCursos()
        {
            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var cursos = await _cursosRepository.GetCursoVistos(user.Id);

            return Ok(cursos);
        }




        [HttpGet("GetVotos")]
        public async Task<IActionResult> GetVotos([FromQuery] int cursoId)
        {
            var votos = await _cursosRepository.GetVotos(cursoId);
            return Ok(votos);
        }




        [HttpPost("Add")]
        public async Task<ActionResult> AddCurso(CursoDto curso)
        {
            var newCurso = new Curso
            {
                AuthorId = curso.Author,
                Titulo = curso.Titulo,
                Video = curso.Video,
            };

            var temas = new List<Temas>();
            foreach (var t in curso.Temas)
            {
                var tema = await _temasRepository.GetByID(t.Id);
                if (tema != null)
                {
                    temas.Add(tema);
                }
            }

            newCurso.Temas = temas;
            var result = await _genericRepository.Add(newCurso);

            if (result == 0)
            {
                return BadRequest();
            }

            return Ok(newCurso);
        }



        [HttpPost("UpdateCurso/{id}")]
        public async Task<ActionResult> UpdateCurso(int id, Curso curso)
        {
            curso.Id = id;
            var result = await _genericRepository.Update(curso);
            if (result == 0)
            {
                return BadRequest();
            }


            return Ok(curso);
        }



        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var curso = await _cursosRepository.GetByID(id);

            if (curso == null)
            {
                return NotFound("Curso not found");
            }
            var author = await _userManager.FindByIdAsync(curso.AuthorId);

            var details = _mapper.Map<Curso, CursoDetailDto>(curso);
            details.Author = author;
            return Ok(details);

        }


        [HttpDelete("DeleteByID/{id}")]
        public async Task<IActionResult> DeleteByID(int id)
        {
            var usuario = await _userManager.BuscarUsuarioAsync(HttpContext.User);
            if (usuario == null)
            {
                return NotFound("Usuario not found");
            }

            var curso = await _genericRepository.GetByID(id);
            if (curso == null)
            {
                return NotFound("Curso not found");
            }

            if (curso.AuthorId != usuario.Id || usuario.Admin)
            {

                return Unauthorized();

            }


            var result = await _genericRepository.DeleteEntity(curso);


            return Ok(result);



        }



    }
}
