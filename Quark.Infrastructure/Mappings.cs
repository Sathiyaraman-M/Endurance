using AutoMapper;
using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Audit;
using Quark.Core.Responses.Identity;
using Quark.Infrastructure.Models.Audit;
using Quark.Infrastructure.Models.Identity;

namespace Quark.Infrastructure.Mappings;

public class AuditProfile : Profile
{
    public AuditProfile()
    {
        CreateMap<AuditResponse, Audit>().ReverseMap();
    }
}
public class RoleClaimProfile : Profile
{
    public RoleClaimProfile()
    {
        CreateMap<RoleClaimResponse, ApplicationRoleClaim>()
            .ForMember(nameof(ApplicationRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
            .ForMember(nameof(ApplicationRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
            .ReverseMap();

        CreateMap<RoleClaimRequest, ApplicationRoleClaim>()
            .ForMember(nameof(ApplicationRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
            .ForMember(nameof(ApplicationRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
            .ReverseMap();
    }
}
public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleResponse, ApplicationRole>().ReverseMap();
    }
}