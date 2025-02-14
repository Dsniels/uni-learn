using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using uni.learn.api.EntitiesDto;
using uni.learn.api.Extensions;
using uni.learn.BussinesLogic.Logic;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api.Controllers
{

    public class UserController : BaseController
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGenericSecurityRepository<Usuario> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        public UserController(
            IMapper mapper,
            IPasswordHasher<Usuario> passwordHasher,
            UserManager<Usuario> userManager,
            IGenericSecurityRepository<Usuario> userRepository,
            SignInManager<Usuario> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userManager = userManager;
        }



        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto login)
        {

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return NotFound("User Not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, true);
            if (!result.Succeeded)
            {
                return Unauthorized(result.IsNotAllowed);
            }


            var roles = await _userManager.GetRolesAsync(user);



            return Ok(new
            {
                Token = _tokenService.CreateToken(user, roles)
            });
        }


        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(RegistroDTO registro)
        {
            var usuario = new Usuario
            {
                Nombre = registro.Nombre,
                ApellidoPaterno = registro.ApellidoPaterno,
                ApellidoMaterno = registro.ApellidoMaterno,
                Email = registro.Email,
                UserName = registro.UserName
            };


            var result = await _userManager.CreateAsync(usuario, registro.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToList());
            }


            return Ok(new
            {
                token = _tokenService.CreateToken(usuario, null)
            });
        }


        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            var result = await _userRepository.GetAllAsync();

            var data = _mapper.Map<IReadOnlyCollection<Usuario>, IReadOnlyCollection<UsuarioDto>>(result);


            return Ok(data);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("AddRoleToUser/{id}")]
        public async Task<ActionResult> AddRoleToUser(string id, RoleDto roleParams)
        {

            var role = await _roleManager.FindByNameAsync(roleParams.Name);
            if (role == null)
            {
                return NotFound("Role does not exists");
            }


            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound("User not found");
            }

            var usuarioDto = _mapper.Map<Usuario, UsuarioDto>(usuario);

            if (roleParams.Status)
            {
                var result = await _userManager.AddToRoleAsync(usuario, roleParams.Name);
                if (result.Succeeded)
                {
                    if (roleParams.Name == "ADMIN")
                    {
                        usuario.Admin = true;
                    }

                }
                if (result.Errors.Any())
                {
                    if (result.Errors.Where(x => x.Code == "UserAlreadyInRole").Any())
                    {
                        if (roleParams.Name == "ADMIN")
                        {
                            usuario.Admin = true;
                        }

                    }
                }

            }
            else
            {
                var result = await _userManager.RemoveFromRoleAsync(usuario, roleParams.Name);
                if (result.Succeeded)
                {
                    if (roleParams.Name == "ADMIN")
                    {
                        usuario.Admin = false;
                    }

                }
            }
            var resultUpdate = await _userManager.UpdateAsync(usuario);

            return Ok();

        }




        [HttpGet("GetUserByID/{id}")]
        public async Task<ActionResult> GetUserByID(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Usuario, UsuarioDto>(user);

            return Ok(result);
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> GetMyProfile()
        {
            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = _mapper.Map<Usuario, UsuarioDto>(user);
            result.Admin = roles.Contains("ADMIN") ? true : false;
            return Ok(result);
        }



        [Authorize]
        [HttpPut("account/updateMyAccount")]
        public async Task<ActionResult> UpdateMyAccount(RegistroDTO dataset)
        {
            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound();
            }
            user.Nombre = dataset.Nombre;
            user.ApellidoPaterno = dataset.ApellidoPaterno;
            user.ApellidoMaterno = dataset.ApellidoMaterno;
            user.Matricula = dataset.Matricula;
            user.Foto = dataset.Imagen;


            if (!string.IsNullOrEmpty(dataset.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, dataset.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }


            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<Usuario, UsuarioDto>(user);
            userDto.Admin = roles.Contains("ADMIN") ? true : false;

            return Ok(userDto);



        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("account/updateUserInfo/{id}")]
        public async Task<ActionResult> UpdateUserInfo(string id, RegistroDTO dataset)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Nombre = dataset.Nombre;
            user.ApellidoPaterno = dataset.ApellidoPaterno;
            user.ApellidoMaterno = dataset.ApellidoMaterno;


            if (!string.IsNullOrEmpty(dataset.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, dataset.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }


            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<Usuario, UsuarioDto>(user);
            userDto.Admin = roles.Contains("ADMIN") ? true : false;

            return Ok(userDto);

        }









    }
}
