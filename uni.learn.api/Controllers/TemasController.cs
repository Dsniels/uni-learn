using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uni.learn.api.EntitiesDto;
using uni.learn.core.Entities;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{
    public class TemasController : BaseController
    {
        private readonly IGenericRepository<Temas> _temasRepository;

        public TemasController(IGenericRepository<Temas> temasRepository)
        {
            _temasRepository = temasRepository;
        }


        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> AddTemas([FromBody] TemaDto dataset)
        {
            var tema = new Temas
            {
                Nombre = dataset.Name
            };

            var result = await _temasRepository.Add(tema);

            if (result == 0)
            {
                return BadRequest("Tema already exists");
            }

            return Ok(tema);
        }



        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllTemas()
        {
            var result = await _temasRepository.GetAllAsync();
            return Ok(result);
        }




        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _temasRepository.GetByID(id);

            if (result == null)
            {
                return NotFound("Tema not found");
            }


            return Ok(result);
        }


        [Authorize(Roles = "ADMIN")]
        [HttpDelete("DeleteByID/{id}")]
        public async Task<IActionResult> DeleteByID(int id)
        {
            var tema = await _temasRepository.GetByID(id);

            if (tema == null)
            {
                return NotFound("Tema not found");
            }
            var result = await _temasRepository.DeleteEntity(tema);
            return Ok(result);
        }
    }
}
