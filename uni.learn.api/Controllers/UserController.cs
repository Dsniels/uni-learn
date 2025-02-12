using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{

    public class UserController : ControllerBase
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;


        public UserController(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userManager = userManager;
        }



        [HttpPost("Login")]
        public async Task<ActionResult> Login(){
            return Ok();
        }
    }
}
