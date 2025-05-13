using AutoMapper;
using ch_13_configuration.Entities;
using ch_13_configuration.Entities.DTOs;

namespace ch_13_configuration.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDtoForInsertion,Book>().ReverseMap();
        CreateMap<BookDtoForUpdate, Book>().ReverseMap();
    }
}