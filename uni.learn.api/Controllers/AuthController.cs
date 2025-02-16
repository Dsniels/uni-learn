using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using uni.learn.api.EntitiesDto;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{
    public class AuthController : ControllerBase
    {
        public readonly UserManager<Usuario> _userManager;
        public readonly SignInManager<Usuario> _signInManager;
        public readonly ITokenService _tokenService;
        public readonly RoleManager<Usuario> _roleManager;
        public readonly IPasswordHasher<Usuario> _passwordHasher;



        public AuthController(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            ITokenService tokenService,
            RoleManager<Usuario> roleManager,
            IPasswordHasher<Usuario> passwordHasher
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto credentials)
        {


            var user = await _userManager.FindByEmailAsync(credentials.Email);

            if (user == null)
            {
                return NotFound("User doesnt exists");
            }


            var result = await _signInManager.CheckPasswordSignInAsync(user, credentials.Password, true);

            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }


            var roles = await _userManager.GetRolesAsync(user);


            return Ok(new
            {
                usuario = new UsuarioDto
                {
                    Nombre = user.Nombre,
                    ApellidoPaterno = user.ApellidoPaterno,
                    Email = user.Email,
                    Admin = roles.Contains("ADMIN")
                },
                token = _tokenService.CreateToken(user, roles)
            });

        }




        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(RegistroDTO registro)
        {
            var user = new Usuario
            {
                Nombre = registro.Nombre,
                ApellidoPaterno = registro.ApellidoPaterno,
                ApellidoMaterno = registro.ApellidoMaterno,
                Email = registro.Email,
                UserName = registro.UserName
            };


            var result = await _userManager.CreateAsync(user, registro.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToList());
            }


            return Ok(new
            {
                usuario = new UsuarioDto
                {
                    Nombre = user.Nombre,
                    ApellidoPaterno = user.ApellidoPaterno,
                    Email = user.Email,
                    Admin = false
                },
                token = _tokenService.CreateToken(user,null)
            });
        }










    }
}





