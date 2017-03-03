using System.Collections.Generic;
using System.Linq;
using Domain;
using NHibernate;
using NHibernate.Linq;

namespace EZVet.Daos
{
    public interface IDecodesDao
    {
        IEnumerable<T> Query<T>() where T : Decode;
        T Get<T>(string name) where T : Decode;

        T Get<T>(int? id) where T : Decode;
    }

    public class DecodesDao : IDecodesDao
    {
        private readonly ISession _session;

        public DecodesDao(ISession session)
        {
            _session = session;
        }

        public T Get<T>(string name) where T : Decode
        {
            return _session.Query<T>().Single(decode => decode.Name == name);
        }

        public T Get<T>(int? id) where T : Decode
        {
            return _session.Query<T>().Single(encode => encode.Id == id);
        }

        public IEnumerable<T> Query<T>() where T : Decode
        {
            return _session.Query<T>();
        }
    }
}