using System.Linq.Expressions;

namespace _01_Framework.Application.Pagination
{
    public record FilteringOptions<T>(Expression<Func<T, bool>> FilteringWhere);
}
