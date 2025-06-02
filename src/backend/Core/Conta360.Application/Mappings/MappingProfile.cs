using AutoMapper;
using Conta360.Application.DTOs;
using Conta360.Application.Features.Accounts.Commands.CreateAccount;
using Conta360.Domain.Entities;

namespace Conta360.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<CreateAccountCommand, Account>();
            // Add more mappings as needed
        }
    }
}