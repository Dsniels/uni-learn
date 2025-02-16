using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using uni.learn.api.EntitiesDto;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<Usuario> _passwordHasher;



        public AuthController(
            IMapper mapper,
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<Usuario> passwordHasher
        )
        {
            _mapper = mapper;
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
                return Unauthorized("Password or Email are incorrect");
            }


            var roles = await _userManager.GetRolesAsync(user);


            var newUser = _mapper.Map<Usuario, UsuarioDto>(user);
            return Ok(new LoginResponse
            {
                Usuario = newUser,
                Admin = roles.Contains("ADMIN"),
                Token = _tokenService.CreateToken(user, roles)
            });

        }




        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(RegistroDTO registro)
        {
            var user = new Usuario
            {
                Nombre = registro.Nombre,
                ApellidoPaterno = registro.ApellidoPaterno,
                Matricula = registro.Matricula,
                ApellidoMaterno = registro.ApellidoMaterno,
                Email = registro.Email,
                UserName = registro.UserName
            };


            var result = await _userManager.CreateAsync(user, registro.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToList());
            }

            var newUser = _mapper.Map<Usuario, UsuarioDto>(user);
            return Ok(new LoginResponse
            {
                Usuario = newUser,
                Admin = false,
                Token = _tokenService.CreateToken(user, null)
            });

        }










    }
}





