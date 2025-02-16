using System;

namespace uni.learn.api.EntitiesDto;


    public class LoginResponse
    {
        public UsuarioDto Usuario { get; set; }
        public bool Admin { get; set; }
        public string Token { get; set; }
    }
