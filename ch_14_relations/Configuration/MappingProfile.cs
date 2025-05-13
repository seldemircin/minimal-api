using AutoMapper;
using ch_14_relations.Entities;
using ch_14_relations.Entities.DTOs;

namespace ch_14_relations.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDtoForInsertion,Book>().ReverseMap();
        CreateMap<BookDtoForUpdate, Book>().ReverseMap();
        CreateMap<BookDto, Book>().ReverseMap();
    }
}