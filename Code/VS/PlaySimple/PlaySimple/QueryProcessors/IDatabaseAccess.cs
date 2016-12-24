using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySimple.QueryProcessors
{
    interface IDatabaseAccess<T> where T : Entity
    {
        T Get(int id);
        void Update(int id, T entity);
        T Save(T entity);
        IQueryable<T> Query();
    }
}
