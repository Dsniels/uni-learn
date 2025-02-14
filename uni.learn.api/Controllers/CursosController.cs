using System.Security.AccessControl;
using AutoMapper;
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
        private readonly IGenericRepository<Curso> _cursoRepository;
        private readonly UserManager<Usuario> _userManager;
        private readonly IGenericRepository<Temas> _temasRepository;
        private readonly ICursoRepository _repository;
        private readonly IMapper _mapper;

        public CursosController(IMapper mapper,UserManager<Usuario> userManager, IGenericRepository<Temas> temasRepository, IGenericRepository<Curso> cursoRepository, ICursoRepository repository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _temasRepository = temasRepository;
            _repository = repository;
            _cursoRepository = cursoRepository;
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCursos()
        {
            var cursos = await _repository.GetAll();

            return Ok(cursos);
        }


        [HttpPost("Add")]
        public async Task<ActionResult> AddCurso(CursoDto curso)
        {
            var newCurso = new Curso
            {
                Author = curso.Author,
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
            var result = await _cursoRepository.Add(newCurso);

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
            var result = await _cursoRepository.Update(curso);
            if (result == 0)
            {
                return BadRequest();
            }


            return Ok(curso);
        }

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var curso = await _repository.GetByID(id);

            if (curso == null)
            {
                return NotFound("Curso not found");
            }
            var author = await _userManager.FindByIdAsync(curso.Author);
            
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

            var curso = await _cursoRepository.GetByID(id);
            if (curso == null)
            {
                return NotFound("Curso not found");
            }

            if (curso.Author != usuario.Id || usuario.Admin)
            {

                return Unauthorized();

            }


            var result = await _cursoRepository.DeleteEntity(curso);


            return Ok(result);



        }



    }
}
