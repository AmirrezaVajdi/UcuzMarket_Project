﻿using System.Linq.Expressions;

namespace _01_Framework.Application.Pagination
{
    public record SortingOptions<T>(Expression<Func<T, object>> expression, bool IsAscending = false);
}
