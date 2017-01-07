using Domain;
using LinqKit;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using EZVet.Common;

namespace EZVet.QueryProcessors
{
    public interface ICustomersQueryProcessor
    {
        
        bool Exists(string username);
    }

    public class CustomersQueryProcessor : DBAccessBase<Customer>, ICustomersQueryProcessor
    {
        private readonly IDecodesQueryProcessor _decodesQueryProcessor;

        public CustomersQueryProcessor(ISession session, IDecodesQueryProcessor decodesQueryProcessor) : base(session)
        {
            _decodesQueryProcessor = decodesQueryProcessor;
        }

        public bool Exists(string username)
        {
            return Query().Where(user => user.Username == username).Any();
        }

    }
}