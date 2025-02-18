using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using uni.learn.api.EntitiesDto;
using uni.learn.api.Extensions;
using uni.learn.core.Entities;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{

    public class VotosController : BaseController
    {
        private readonly IVotosRepostory _votosRepostory;
        private readonly IGenericRepository<Voto> _genericRepository;
        private readonly UserManager<Usuario> _userManager;


        public VotosController(
            IVotosRepostory votosRepostory,
            IGenericRepository<Voto> genericRepository,
            UserManager<Usuario> userManager
        )
        {
            _genericRepository = genericRepository;
            _userManager = userManager;
            _votosRepostory = votosRepostory;

        }



        [HttpGet("GetVotos")]
        public async Task<IActionResult> GetVotos([FromQuery] int cursoId)
        {
            var votos = await _votosRepostory.GetVotos(cursoId);
            return Ok(votos);
        }


        [Authorize]
        [HttpGet("UserLikedCurso")]
        public async Task<IActionResult> UserLikedCurso([FromQuery] int cursoId)
        {
            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _votosRepostory.UserLikeCurso(user.Id, cursoId);

            return Ok(result);

        }

        [Authorize]
        [HttpPost("likeCurso")]
        public async Task<IActionResult> LikeCurso([FromQuery] int cursoId)
        {
            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }

            var currVoto = await _votosRepostory.GetVotoAsync(user.Id, cursoId);

            if (currVoto != null)
            {
                return Ok();
            }
            var newVoto = new Voto
            {
                CursoId = cursoId,
                UsuarioId = user.Id,
                Like = true,
                FechaVoto = DateTime.UtcNow
            };
            await _genericRepository.Add(newVoto);


            return Ok();

        }

        [Authorize]
        [HttpPost("DisLikeCurso")]
        public async Task<IActionResult> DisLikeVideo([FromQuery] int cursoId){
            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }

            var currVoto = await _votosRepostory.GetVotoAsync(user.Id, cursoId);

            if (currVoto != null)
            {
                return Ok();
            }

            var result = await _genericRepository.DeleteEntity(currVoto);

            if(result == 0){
                return BadRequest();
            }

            return Ok();
        }


        [Authorize]
        [HttpPost("UpdateVoto")]
        public async Task<IActionResult> UpdateVoto(UpdateVotoDto voto)
        {

            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }

            var currVoto = await _votosRepostory.GetVotoAsync(user.Id, voto.CursoId);

            if (currVoto != null)
            {
                currVoto.Like = voto.like;
                currVoto.FechaVoto = DateTime.UtcNow;
                await _genericRepository.Update(currVoto);
            }
            else
            {
                var newVoto = new Voto
                {
                    CursoId = voto.CursoId,
                    UsuarioId = user.Id,
                    Like = voto.like,
                    FechaVoto = DateTime.UtcNow
                };
                await _genericRepository.Add(newVoto);
            }


            return Ok();


        }


    }
}
