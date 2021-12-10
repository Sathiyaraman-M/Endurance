using Quark.Core.Features.Books.Commands;
using Quark.Core.Features.Checkouts.Commands;
using Quark.Core.Features.Patrons.Commands;

namespace Quark.Core;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<AddEditBookCommand, Book>();
        CreateMap<BookResponse, Book>().ReverseMap();
        CreateMap<BookHeaderResponse, BookHeader>().ReverseMap();
    }
}
public class CheckoutProfile : Profile
{
    public CheckoutProfile()
    {
        CreateMap<AddCheckoutCommand, Checkout>();
    }
}
public class PatronProfile : Profile
{
    public PatronProfile()
    {
        CreateMap<AddEditPatronCommand, Patron>()
            .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => src.DateOfBirth ?? DateTime.MinValue))
            .ReverseMap();
        CreateMap<Patron, PatronResponse>().ReverseMap();
    }
}

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<PermissionRequest, PermissionResponse>().ReverseMap();
        CreateMap<RoleClaimRequest, RoleClaimResponse>().ReverseMap();
    }
}