using AutoMapper;
using ch_15_identity.Entities;
using ch_15_identity.Entities.DTOs;
using ch_15_identity.Entities.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace ch_15_identity.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDtoForInsertion,Book>().ReverseMap();
        CreateMap<BookDtoForUpdate, Book>().ReverseMap();
        CreateMap<BookDto, Book>().ReverseMap();
        
        CreateMap<UserDtoForRegister, User>().ReverseMap();
    }
}