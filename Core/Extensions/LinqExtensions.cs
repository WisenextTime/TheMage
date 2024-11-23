using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TheMage.Core.Extensions;

public static class LinqExtensions
{
    public static TValue Sum<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> selector)
        where TValue : IAdditionOperators<TValue, TValue, TValue> =>
        source.Select(selector).Aggregate((a, b) => a + b);

    public static TValue Sum<TValue>(this IEnumerable<TValue> source)
        where TValue : IAdditionOperators<TValue, TValue, TValue> =>
        source.Aggregate((a, b) => a + b);
}