global using AutoMapper;
global using FluentValidation;
global using LazyCache;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Quark.Core.Domain.Common;
global using Quark.Core.Domain.Entities;
global using Quark.Core.Extensions;
global using Quark.Core.Interfaces.Repositories;
global using Quark.Core.Interfaces.Services;
global using Quark.Core.Requests.Identity;
global using Quark.Core.Responses;
global using Quark.Core.Responses.Identity;
global using Quark.Core.Specifications;
global using Quark.Core.Specifications.Base;
global using Quark.Shared;
global using Quark.Shared.Constants;
global using Quark.Shared.Wrapper;
global using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Reflection;

namespace Quark.Core.Extensions;

public static class EnumExtensions
{
    public static string ToDescriptionString(this Enum val)
    {
        var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0
            ? attributes[0].Description
            : val.ToString();
    }
}
public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
    {
        if (source == null) throw new ApiException();
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
    {
        var queryableResultWithIncludes = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        var secondaryResult = spec.IncludeStrings.Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));
        return secondaryResult.Where(spec.Criteria);
    }
}
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}