using Domain;
using LinqKit;
using NHibernate;
using PlaySimple.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.QueryProcessors
{
    public interface ICustomersQueryProcessor
    {
        IEnumerable<DTOs.Customer> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? customerId);

        DTOs.Customer GetCustomer(int id);

        DTOs.Customer Save(DTOs.Customer customer);

        DTOs.Customer Update(int id, DTOs.Customer customer);

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

        public IEnumerable<DTOs.Customer> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? customerId)
        {
            var filter = PredicateBuilder.New<Customer>(x => true);

            if (!string.IsNullOrEmpty(firstName))
            {
                filter.And(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                filter.And(x => x.LastName.Contains(lastName));
            }

            if (minAge.HasValue)
            {
                filter.And(x => x.BirthDate <= DateUtils.GetXYearsEarly(minAge??0));
            }

            if (maxAge.HasValue)
            {
                filter.And(x => x.BirthDate >= DateUtils.GetXYearsEarly(maxAge??0));
            }

            if (region.HasValue)
            {
                RegionDecode regionDecode = _decodesQueryProcessor.Get<RegionDecode>(region);
                filter.And(x => x.Region == regionDecode);
            }

            if (customerId.HasValue)
            {
                filter.And(x => x.Id == customerId);
            }
            var result = Query().Where(filter).ToList().Select(x => new DTOs.Customer().Initialize(x));

            return result;
        }

        public DTOs.Customer GetCustomer(int id)
        {
            return new DTOs.Customer().Initialize(Get(id));
        }

        public DTOs.Customer Save(DTOs.Customer customer)
        {
            Customer newCustomer = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Username = customer.Username,
                Password = customer.Password,
                BirthDate = customer.BirthDate,
                Email = customer.Email,
                Region = _decodesQueryProcessor.Get<RegionDecode>(customer.Region),
                FreezeDate = customer.FreezeDate
            };

            Customer persistedCustomer = Save(newCustomer);

            return new DTOs.Customer().Initialize(persistedCustomer);
        }

        public DTOs.Customer Update(int id, DTOs.Customer customer)
        {
            Customer existingCustomer = Get(id);

            existingCustomer.FirstName = customer.FirstName ?? existingCustomer.FirstName;
            existingCustomer.LastName = customer.LastName ?? existingCustomer.LastName;
            existingCustomer.Username = customer.Username ?? existingCustomer.Username;
            existingCustomer.Password = customer.Password ?? existingCustomer.Password;
            existingCustomer.Email = customer.Email ?? existingCustomer.Email;

            if (customer.BirthDate != null)
                existingCustomer.BirthDate = customer.BirthDate;

            if (customer.Region != null)
                existingCustomer.Region = _decodesQueryProcessor.Get<RegionDecode>(customer.Region);

            if (customer.FreezeDate != null)
                existingCustomer.FreezeDate = customer.FreezeDate;

            Update(id, existingCustomer);

            return new DTOs.Customer().Initialize(existingCustomer);
        }
    }
}