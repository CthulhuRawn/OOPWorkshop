using Domain;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.QueryProcessors
{
    public abstract class DBAccessBase<T> : IDatabaseAccess<T> where T : Entity
    {
        private readonly ISession _session;

        public DBAccessBase(ISession session)
        {
            _session = session;
        }

        public T Get(int id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> Query()
        {
            return _session.Query<T>();
        }

        public void Update(int id, T entity)
        {
            _session.Update(entity);
        }

        public T Save(T entity)
        {
            entity.Id = (int)_session.Save(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);
        }
    }

}