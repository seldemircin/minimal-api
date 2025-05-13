using AutoMapper;
using ch_16_jwt.Entities;
using ch_16_jwt.Entities.DTOs.Books;
using ch_16_jwt.Entities.DTOs.Users;

namespace ch_16_jwt.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDtoForInsertion,Book>().ReverseMap();
        CreateMap<BookDtoForUpdate, Book>().ReverseMap();
        CreateMap<BookDto, Book>().ReverseMap();
        
        CreateMap<UserDtoForRegister, User>().ReverseMap();
        CreateMap<UserDtoForAuthentication, User>().ReverseMap();
    }
}