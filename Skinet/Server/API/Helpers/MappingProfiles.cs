﻿using API.Dtos;
using AutoMapper;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Identity;
using Skinet.WebAPI.Dtos;

namespace Skinet.WebAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
            CreateMap<BasketItemDto, BasketItem>();
        }
    }
}
