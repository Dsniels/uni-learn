using System;
using AutoMapper;
using uni.learn.core.Entity;

namespace uni.learn.api.EntitiesDto;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
    }

}
