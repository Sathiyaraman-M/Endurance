using Quark.Shared.Wrapper;

namespace Quark.Client.Managers.Generic;

public interface IHttpClientManager<T, TId>
{
    Task<IResult<List<T>>> GetAllAsync(string route);

    Task<IResult<T>> GetBydIdAsync(TId id, string route);

    Task<IResult<TId>> SaveAsync(T request, string route);

    Task<IResult<TId>> DeleteAsync(TId id, string route);
}
