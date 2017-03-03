using EZVet.Daos;
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
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OwnerMap>())
                    .CurrentSessionContext("web")
                    .ExposeConfiguration(conf => new SchemaUpdate(conf).Execute(false, true))
                    .BuildSessionFactory();
            var session = sessionFactory.OpenSession();
            
            //var decode = new DecodesDao(session);
            //var employeeQP = new EmployeesDao(session);
            //var fieldQP = new FieldsDao(decode, session);
            //var customerQP = new OwnersDao(session);

            //var complaintQP = new ComplaintsDao(session, decode, customerQP);
            //var orderQP = new OrdersDao(session, customerQP, fieldQP, decode);
            //var participantQP = new ParticipantsDao(session, customerQP, orderQP, decode);
            //var reviewQP = new ReviewsDao(session, customerQP);
            
            //ReportsDao reportsQP = new ReportsDao(customerQP, orderQP, complaintQP, participantQP);

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
