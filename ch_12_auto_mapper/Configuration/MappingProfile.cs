using AutoMapper;
using ch_12_auto_mapper.Entities;
using ch_12_auto_mapper.Entities.DTOs;

namespace ch_12_auto_mapper.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDtoForInsertion,Book>().ReverseMap();
        CreateMap<BookDtoForUpdate, Book>().ReverseMap();
    }
}