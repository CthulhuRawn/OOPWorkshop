using System.Linq;
using Domain;

namespace DAL
{
    public interface IDatabaseAccess<T> where T : Entity
    {
        T Get(int id);
        void Update(int id, T entity);
        T Save(T entity);
        IQueryable<T> Query();
    }
}
