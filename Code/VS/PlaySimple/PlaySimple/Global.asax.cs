using Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Maps;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using PlaySimple.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;

namespace PlaySimple
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
