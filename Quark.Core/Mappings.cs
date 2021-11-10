using AutoMapper;
using Quark.Core.Domain.Entities;
using Quark.Core.Features.Books.Commands;
using Quark.Core.Features.Checkouts.Commands;
using Quark.Core.Features.Designations.Commands;
using Quark.Core.Features.Patrons.Commands;
using Quark.Core.Responses;

namespace Quark.Core.Mappings;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<AddEditBookCommand, Book>();
        CreateMap<BookResponse, Book>();
    }
}
public class CheckoutProfile : Profile
{
    public CheckoutProfile()
    {
        CreateMap<AddCheckoutCommand, Checkout>();
    }
}
public class DesignationProfile : Profile
{
    public DesignationProfile()
    {
        CreateMap<AddEditDesignationCommand, Designation>();
        CreateMap<DesignationResponse, Designation>().ReverseMap();
    }
}
public class PatronProfile : Profile
{
    public PatronProfile()
    {
        CreateMap<AddEditPatronCommand, Patron>()
            .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value : DateTime.MinValue))
            .ReverseMap();
        CreateMap<Patron, PatronResponse>();
    }
}