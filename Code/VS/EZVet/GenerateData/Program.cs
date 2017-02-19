using EZVet.QueryProcessors;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Maps;
using NHibernate.Tool.hbm2ddl;

namespace GenerateData
{
    class Program
    {
        static void Main(string[] args)
        {
            var absoluteDbPath = "C:/W/git_managed/Sadna/db.sqlite";

            var sessionFactory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile(absoluteDbPath))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CustomerMap>())
                    .CurrentSessionContext("web")
                    .ExposeConfiguration(conf => new SchemaUpdate(conf).Execute(false, true))
                    .BuildSessionFactory();
            var session = sessionFactory.OpenSession();

            var decode = new DecodesQueryProcessor(session);
            var employeeQP = new EmployeesQueryProcessor(session);
            var fieldQP = new FieldsQueryProcessor(decode, session);
            var customerQP = new OwnersQueryProcessor(session, decode);

            var complaintQP = new ComplaintsQueryProcessor(session, decode, customerQP);
            var orderQP = new OrdersQueryProcessor(session, customerQP, fieldQP, decode);
            var participantQP = new ParticipantsQueryProcessor(session, customerQP, orderQP, decode);
            var reviewQP = new ReviewsQueryProcessor(session, customerQP);

            //ReportsQueryProcessor reportsQP = new ReportsQueryProcessor(customerQP, orderQP, complaintQP, participantQP);

            //IEnumerable<Order> orrr = orderQP.GetAvailbleOrders(2, null, 1, new System.DateTime(2016, 8, 20, 0, 0, 0));
            //IEnumerable<CustomersActivityReport> result = reportsQP.GetCustomersActivityReport(null, null, null, null);
            //Order order = new Order()
            //{
            //    Field = fieldQP.GetField(1),
            //    Owner = customerQP.GetCustomer(1),
            //    StartDate = new System.DateTime(2016, 8, 27, 18, 0, 0),
            //    Status = 1,
            //    PlayersNumber = 2
            //};

            //orderQP.Save(order);
        }
    }
}
