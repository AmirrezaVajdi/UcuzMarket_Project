
using System.Linq.Expressions;

namespace _01_Framework.Domain
{
    public interface IRepository<TKey, T> where T : EntityBase
    {
        T Get(TKey id);
        IEnumerable<T> GetAll();
        void Create(T entity);
        bool Exsists(Expression<Func<T, bool>> expression);
        void SaveChanges();
    }
}
