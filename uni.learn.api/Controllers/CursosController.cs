using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using uni.learn.api.EntitiesDto;
using uni.learn.api.Extensions;
using uni.learn.core.Entities;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;
using uni.learn.core.Specifications;

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


        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCursos([FromQuery] CursoSpecificationParams cursosParams)
        {
            var spec = new CursoSpecifications(cursosParams);
            var cursos = await _genericRepository.GetAllWithSpec(spec);

            return Ok(cursos);
        }

        [HttpGet("GetApprovedCursos")]
        public async Task<IActionResult> GetApprovedCursos([FromQuery] CursoSpecificationParams cursosParams)
        {
            var spec = new CursoSpecifications(cursosParams);
            var cursos = await _cursosRepository.GetApprovedCursos(spec);
            //var cursosList = _mapper.Map<IReadOnlyCollection<Curso>, IReadOnlyCollection<CursoDto>>(cursos);
            return Ok(cursos);
        }


        [Authorize(Roles="ADMIN")]
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
                Descripcion = curso.Descripcion
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

            return Ok(newCurso);
        }


        [Authorize]
        [HttpPost("UpdateCurso/{id}")]
        public async Task<ActionResult> UpdateCurso(int id, Curso cursoUpdate)
        {
            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);
 

            var cursoOriginal = await _genericRepository.GetByID(id);
            if (cursoOriginal == null)
            {
                return NotFound("Curso not Found");
            }

            if (cursoUpdate.Aprobado != cursoOriginal.Aprobado && !user.Admin)
            {
                return Unauthorized("Only an admin can approve this video");
            }

            cursoOriginal.Titulo = cursoUpdate.Titulo;
            cursoOriginal.Video = cursoUpdate.Video;
            cursoOriginal.Descripcion = cursoUpdate.Descripcion;
            if (user.Admin)
            {
                cursoOriginal.Aprobado = cursoUpdate.Aprobado;
            }

            var result = await _genericRepository.Update(cursoOriginal);
            if (result == 0)
            {
                return BadRequest();
            }

            return Ok(cursoOriginal);
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
            var authorDto = _mapper.Map<Usuario, UsuarioDto>(author);

            var details = _mapper.Map<Curso, CursoDetailDto>(curso);
            details.Author = authorDto;
            return Ok(details);

        }

        [Authorize]
        [HttpDelete("DeleteByID/{id}")]
        public async Task<IActionResult> DeleteByID(int id)
        {
            var usuario = await _userManager.BuscarUsuarioAsync(HttpContext.User);
      

            var curso = await _genericRepository.GetByID(id);
            if (curso == null)
            {
                return NotFound("Curso not found");
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            if (curso.AuthorId != usuario.Id || roles.Contains("ADMIN") ? false: true)
            {
                return Unauthorized();
            }

            var result = await _genericRepository.DeleteEntity(curso);
            return Ok(result);

        }



    }
}
