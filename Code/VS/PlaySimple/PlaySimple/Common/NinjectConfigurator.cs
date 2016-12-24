using Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Maps;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using PlaySimple.QueryProcessors;
using System;
using System.Web;

namespace PlaySimple.Common
{
    public class NinjectConfigurator
    {
        public void Configure(IKernel container)
        {
            AddBindings(container);
        }

        public void AddBindings(IKernel container)
        {
            ConfigureNhibernate(container);
            ConfigureQueryProcessors(container);
        }

        private void ConfigureQueryProcessors(IKernel container)
        {
            container.Bind<IComplaintsQueryProcessor>().To<ComplaintsQueryProcessor>();
            container.Bind<ICustomersQueryProcessor>().To<CustomersQueryProcessor>();
            container.Bind<IDecodesQueryProcessor>().To<DecodesQueryProcessor>();
            container.Bind<IEmployeesQueryProcessor>().To<EmployeesQueryProcessor>();
            container.Bind<IFieldsQueryProcessor>().To<FieldsQueryProcessor>();
            container.Bind<IOrdersQueryProcessor>().To<OrdersQueryProcessor>();
            container.Bind<IParticipantsQueryProcessor>().To<ParticipantsQueryProcessor>();
            container.Bind<IReportsQueryProcessor>().To<ReportsQueryProcessor>();
            container.Bind<IReviewsQueryProcessor>().To<ReviewsQueryProcessor>();
        }

        private void ConfigureNhibernate(IKernel container)
        {
            container.Bind<ISessionFactory>().ToMethod(CreateSessionFactory).InSingletonScope();

            container.Bind<ISession>().ToMethod(CreateSession).InRequestScope();
        }

        private ISessionFactory CreateSessionFactory(IContext context)
        {
            string absoluteDbPath = HttpContext.Current.Server.MapPath(Consts.DB_PATH);

            return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile(absoluteDbPath))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CustomerMap>())
                    .CurrentSessionContext("web")
                    .ExposeConfiguration(conf => new SchemaUpdate(conf).Execute(false, true))
                    .BuildSessionFactory();
        }

        private ISession CreateSession(IContext context)
        {
            var sessionFactory = context.Kernel.Get<ISessionFactory>();

            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

            return sessionFactory.GetCurrentSession();
        }
    }
}