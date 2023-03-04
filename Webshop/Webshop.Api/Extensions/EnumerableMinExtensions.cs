namespace Webshop.Api.Extensions;

public static class EnumerableMinExtensions
{
    public static TResult? MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        if (!source.Any())
            return default;

        return source.Min(selector);
    }

    public static double? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
        if (!source.Any())
            return default;

        return source.Min(selector);
    }
}
