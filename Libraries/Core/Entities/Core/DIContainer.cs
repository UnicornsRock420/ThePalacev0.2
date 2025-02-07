using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using Microsoft.Extensions.Configuration;
using ThePalace.Logging.Entities;
using ILogger = Serilog.ILogger;

namespace ThePalace.Core.Entities.Core
{
    public partial class DIContainer
    {
        #region cStr
        public DIContainer()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<DIContainer>()
                .Build();

            _logger = new LoggingHub(_configuration);

            Builder = new ContainerBuilder();
        }
        public DIContainer(ContainerBuilder builder) : this()
        {
            Builder = builder;
        }
        public DIContainer(IConfiguration configuration) : this()
        {
            _configuration = configuration;
        }
        public DIContainer(ContainerBuilder builder, IConfiguration configuration) : this()
        {
            _configuration = configuration;

            Builder = builder;
        }
        #endregion

        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private static ILogger _logger;
        public ContainerBuilder Builder { get; private set; }
        public IContainer? Container { get; private set; }
        #endregion

        #region Register Methods
        public DIContainer RegisterModules<TModule>(IEnumerable<TModule> modules)
            where TModule : Module
        {
            foreach (var module in modules)
                Builder.RegisterModule(module);
            return this;
        }

        public DIContainer RegisterModules<TModule>(params TModule[] modules)
            where TModule : Module
        {
            foreach (var module in modules)
                Builder.RegisterModule(module);
            return this;
        }

        public DIContainer RegisterInstances<TInstance>(IEnumerable<TInstance> instances)
            where TInstance : Type
        {
            Builder
                .RegisterInstance(instances)
                .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterInstances<TInstance, TAs>(IEnumerable<TInstance> instances)
            where TInstance : Type
            where TAs : Type
        {
            Builder
                .RegisterInstance(instances)
                .As<TAs>()
                .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterInstances<TInstance>(params TInstance[] instances)
            where TInstance : Type
        {
            Builder
                .RegisterInstance(instances)
                .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterInstances<TInstance, TAs>(params TInstance[] instances)
            where TInstance : Type
            where TAs : Type
        {
            Builder
                .RegisterInstance(instances)
                .As<TAs>()
                .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterType<TType>(TType type, List<Parameter> @params)
            where TType : Type
        {
            Builder
                .RegisterType(type)
                .WithParameters(@params)
                .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterType<TType, TAs>(TType type, List<Parameter> @params)
            where TType : Type
            where TAs : Type
        {
            Builder
                .RegisterType(type)
                .WithParameters(@params)
                .As<TAs>()
                .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterTypes<TType>(IEnumerable<TType> types)
            where TType : Type
        {
            foreach (var type in types)
                Builder
                    .RegisterType(type)
                    .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterTypes<TType, TAs>(IEnumerable<TType> types)
            where TType : Type
            where TAs : Type
        {
            foreach (var type in types)
                Builder
                    .RegisterType(type)
                    .As<TAs>()
                    .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterTypes<TType>(params TType[] types)
            where TType : Type
        {
            foreach (var type in types)
                Builder
                    .RegisterType(type)
                    .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterTypes<TType, TAs>(params TType[] types)
            where TType : Type
            where TAs : Type
        {
            if (!typeof(TAs).IsInterface) return this;

            foreach (var type in types)
                Builder
                    .RegisterType(type)
                    .As<TAs>()
                    .InstancePerLifetimeScope();
            return this;
        }

        public DIContainer RegisterServices<TService>(IEnumerable<TService> services, IResolveMiddleware middleware, MiddlewareInsertionMode insertionMode = MiddlewareInsertionMode.EndOfPhase)
            where TService : Service
        {
            foreach (var service in services)
                Builder.RegisterServiceMiddleware(service, middleware, insertionMode);
            return this;
        }
        #endregion

        public IContainer? Build()
        {
            try
            {
                Container?.Dispose();
            }
            catch
            {
                Container = null;
            }

            try
            {
                Container = Builder.Build();
            }
            catch
            {
                Container = null;
            }


            return Container;
        }

        public TType? Resolve<TType>()
            where TType : Type
        {
            if (Container == null)
            {
                Build();
            }

            return Container?.Resolve<TType>();
        }
    }
}