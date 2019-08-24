
using Owin;
using ReadLater.Data;
using ReadLater.Repository;
using ReadLater.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace MVC
{
    public partial class Startup
    {
        public static void ConfigureSimpleInjector(IAppBuilder app, Container container)
        {
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
        private static void InitializeContainer(Container container)
        {
            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>();

            container.Register<IDbContext, ReadLaterDataContext>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);

            //services

            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
            container.Register<IBookmarkService, BookmarkService>(Lifestyle.Scoped);

            container.Register<IClientService, ClientService>(Lifestyle.Scoped);
        }

    }
}