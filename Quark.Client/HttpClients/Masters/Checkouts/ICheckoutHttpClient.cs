using Quark.Core.Features.Checkouts.Commands;
using Quark.Core.Requests;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Client.HttpClients.Masters.Checkouts;

public interface ICheckoutHttpClient
{
    Task<PaginatedResult<CheckoutResponse>> GetAllPaginatedAsync(PagedRequest request);

    Task<IResult<CheckoutResponse>> GetByIdAsync(int id);

    Task<IResult<int>> AddCheckoutAsync(AddCheckoutCommand command);

    Task<IResult<int>> ExtendDaysAsync(ExtendCheckoutCommand command);

    Task<IResult<int>> CheckInAsync(CheckInBookCommand command);

    Task<IResult<int>> DeleteCheckoutAsync(int id);
}