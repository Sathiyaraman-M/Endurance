namespace Quark.Client.HttpClients.Generic;

public interface IGenericHttpClient<T, TId>
{
    Task<IResult<List<T>>> GetAllAsync(string route);

    Task<IResult<T>> GetBydIdAsync(TId id, string route);

    Task<IResult<TId>> SaveAsync(T request, string route);

    Task<IResult<TId>> DeleteAsync(TId id, string route);
}