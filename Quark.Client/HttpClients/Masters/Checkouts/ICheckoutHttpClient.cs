using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Client.HttpClients.Masters.Checkouts;

public interface ICheckoutHttpClient
{
    Task<PaginatedResult<CheckoutResponse>> GetAllPaginatedAsync(PagedRequest request);

    Task<IResult<CheckoutResponse>> GetByIdAsync(Guid id);

    Task<IResult<Guid>> AddCheckoutAsync(AddCheckoutCommand command);

    Task<IResult<Guid>> ExtendDaysAsync(ExtendCheckoutCommand command);

    Task<IResult<Guid>> CheckInAsync(CheckInBookCommand command);

    Task<IResult<Guid>> DeleteCheckoutAsync(Guid id);
}