using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles() 
    {

        CreateMap<CteProv, CteProvDto>()
            .ReverseMap();

        CreateMap<Producto, ProductoDto>()
            .ReverseMap();

        CreateMap<Reporte, ReporteDto>()
            .ReverseMap();

        CreateMap<Usuario, UsuarioDto>() 
            .ReverseMap(); 
    
    }


}
