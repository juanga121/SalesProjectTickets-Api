using AutoMapper;
using SalesProjectTickets.Application.DTOs;
using SalesProjectTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LoginDTO, LoginUsers>().ReverseMap();
            CreateMap<PurchaseHistory, PaymentProcessDTO>()
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.Users!.Name))
                .ForMember(dest => dest.EmailUser, opt => opt.MapFrom(src => src.Users!.Email))
                .ForMember(dest => dest.NameTicket, opt => opt.MapFrom(src => src.Tickets!.Name));
        }
    }
}
