using System.Web;
using DAL;
using DTO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Maps;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using Ninject.Extensions.Conventions;

namespace EZVet.Common
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
            ConfigureDaos(container);
        }

        private void ConfigureDaos(IKernel container)
        {
            container.Bind(x =>
            {
                x.FromAssemblyContaining<ITreatmentsDao>()
                 .SelectAllClasses()
                 .BindDefaultInterface();
            });
        }

        private void ConfigureNhibernate(IKernel container)
        {
            container.Bind<ISessionFactory>().ToMethod(CreateSessionFactory).InSingletonScope();

            container.Bind<ISession>().ToMethod(CreateSession).InRequestScope();
        }

        private ISessionFactory CreateSessionFactory(IContext context)
        {
            var dbPath = "~/App_Data/db.sqlite";
            var absoluteDbPath = HttpContext.Current.Server.MapPath(dbPath);

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(absoluteDbPath))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OwnerMap>())
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