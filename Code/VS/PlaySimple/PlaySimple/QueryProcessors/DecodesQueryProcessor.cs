using Domain;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.QueryProcessors
{
    public interface IDecodesQueryProcessor
    {
        T Get<T>(string name) where T : Decode;

        T Get<T>(int? id) where T : Decode;
    }

    public class DecodesQueryProcessor : IDecodesQueryProcessor
    {
        private readonly ISession _session;

        public DecodesQueryProcessor(ISession session)
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
    }
}