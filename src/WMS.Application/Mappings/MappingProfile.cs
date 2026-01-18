using AutoMapper;
using WMS.Application.DTOs;
using WMS.Domain.Entities;
using WMS.Domain.ValueObjects;

namespace WMS.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight.Value))
            .ForMember(dest => dest.WeightUnit, opt => opt.MapFrom(src => src.Weight.Unit));

        // Inventory mappings
        CreateMap<Inventory, InventoryDto>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore())
            .ForMember(dest => dest.WarehouseName, opt => opt.Ignore())
            .ForMember(dest => dest.LocationCode, opt => opt.Ignore());

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TotalValue.Currency));

        CreateMap<OrderLine, OrderLineDto>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore())
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Amount))
            .ForMember(dest => dest.LineTotal, opt => opt.MapFrom(src => src.LineTotal.Amount));

        // Address mappings
        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>()
            .ConvertUsing(dto =>
                Address.Create(dto.Street, dto.City, dto.State, dto.PostalCode, dto.Country)
            );
    }
}
