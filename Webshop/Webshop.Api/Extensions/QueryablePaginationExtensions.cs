using Microsoft.EntityFrameworkCore;
using System;

namespace Webshop.Api.Extensions;

public static class QueryablePaginationExtensions
{
    public static List<TSource> ToPaginated<TSource>(this List<TSource> source, int pageNumber, int pageSize)
    {
        var sourceSize = source.Count;
        
        if (sourceSize == 0)
            return source;

        var skip = (pageNumber - 1) * pageSize;

        if (sourceSize <= skip)
            throw new ArgumentOutOfRangeException(nameof(pageNumber));

        return source
            .Skip(skip)
            .Take(pageSize)
            .ToList();
    }
}
