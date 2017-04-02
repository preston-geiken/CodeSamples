using Microsoft.Practices.Unity;
using Bringpro.Web.Classes.SignalR;
using Bringpro.Web.Classes.Tasks.Bringg;
using Bringpro.Web.Classes.Tasks.Bringg.Interfaces;
using Bringpro.Web.Core.Injection;
using Bringpro.Web.Services;
using Bringpro.Web.Services.Interfaces;
using System;
using System.Web.Http;
using System.Web.Mvc;
using Unity.Mvc5;


namespace Bringpro.Web
{
    public static class UnityConfig
    {

        private static readonly Lazy<UnityContainer> _container = new Lazy<UnityContainer>(() => new UnityContainer());

        public static UnityContainer GetContainer()
        {
            return _container.Value;
        }

        public static void RegisterComponents(HttpConfiguration config)
        {
            UnityContainer container = new UnityContainer();

            // register all your components with the container here
          

            //container.RegisterType<IUserEmailService, SendGridService>();
            container.RegisterType<IUserEmailService, MandrillService>();
            container.RegisterType<IUserRegistrationReport, UserRegistrationReport>();
            container.RegisterType<IJobsService, JobsService>();
            container.RegisterType<IJobsWaypointService, JobsWaypointService>();
            container.RegisterType<ITransactionLogService, TransactionLogService>();

            /* Note to GitHub Viewers: Many lines of code have been taken out of this file, so
               I could showcase DI with Bringg Api */
            container.RegisterType(typeof(IBringgTask<>), typeof(CreateUser<>), "CreateUser");
            container.RegisterType(typeof(IBringgTask<>), typeof(UpdateUser<>), "UpdateUser");
            container.RegisterType(typeof(IBringgTask<>), typeof(DeleteUser<>), "DeleteUser");
            container.RegisterType(typeof(IBringgTask<>), typeof(CreateTask<>), "CreateTask");
            container.RegisterType(typeof(IBringgTask<>), typeof(CreateTaskWithWaypoints<>), "CreateTaskWithWaypoints");
            container.RegisterType(typeof(IBringgTask<>), typeof(CreateWaypoint<>), "CreateWaypoint");
            container.RegisterType(typeof(IBringgTask<>), typeof(UpdateCustomerTask<>), "UpdateCustomerTask");
            container.RegisterType(typeof(IBringgTask<>), typeof(UpdateTeamTask<>), "UpdateTeamTask");
            container.RegisterType(typeof(IBringgTask<>), typeof(DeleteTeamTask<>), "DeleteTeamTask");

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            //  this line is needed so that the resolver can be used by api controllers 
            config.DependencyResolver = new Bringpro.Web.Core.Injection.UnityResolver(container);

            var resolver = new UnityDependencyResolver(container);
            //container.RegisterType<IOutputService, OutputService>();


            DependencyResolver.SetResolver(resolver);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));


            //  this line is needed so that the resolver can be used by api controllers 
                        config.DependencyResolver = new UnityResolver(container);

            //  we have to make a custom filter injector to provide DI to our custom action filters.
            //  see http://michael-mckenna.com/blog/dependency-injection-for-asp-net-web-api-action-filters-in-3-easy-steps
            //var providers = config.Services.GetFilterProviders().ToList();

            //var defaultprovider = providers.Single(i => i is ActionDescriptorFilterProvider);
            //config.Services.Remove(typeof(System.Web.Http.Filters.IFilterProvider), defaultprovider);

            config.Services.Add(typeof(System.Web.Http.Filters.IFilterProvider), new UnityActionFilterProvider(container));


        }
    }
}

