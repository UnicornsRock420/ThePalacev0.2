using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;

namespace ThePalace.Core.Entities.DependencyInjection
{
    public partial class Container
    {
        public Container() =>
            Builder = new ContainerBuilder();

        public ContainerBuilder Builder { get; private set; }

        #region Register Methods
        public Container RegisterModules<TModule>(IEnumerable<TModule> modules)
            where TModule : Module
        {
            foreach (var module in modules)
                Builder.RegisterModule(module);
            return this;
        }

        public Container RegisterModules<TModule>(params TModule[] modules)
            where TModule : Module
        {
            foreach (var module in modules)
                Builder.RegisterModule(module);
            return this;
        }

        public Container RegisterInstances<TInstance>(IEnumerable<TInstance> instances)
            where TInstance : Type
        {
            Builder
                .RegisterInstance(instances)
                .InstancePerLifetimeScope();
            return this;
        }

        public Container RegisterInstances<TInstance>(params TInstance[] instances)
            where TInstance : Type
        {
            Builder
                .RegisterInstance(instances)
                .InstancePerLifetimeScope();
            return this;
        }

        public Container RegisterType<TType>(TType type, List<Parameter> @params)
            where TType : Type
        {
            Builder
                .RegisterType(type)
                .WithParameters(@params)
                .InstancePerLifetimeScope();
            return this;
        }

        public Container RegisterType<TType, TAs>(TType type, List<Parameter> @params)
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

        public Container RegisterTypes<TType>(IEnumerable<TType> types)
            where TType : Type
        {
            foreach (var type in types)
                Builder
                    .RegisterType(type)
                    .InstancePerLifetimeScope();
            return this;
        }

        public Container RegisterTypes<TType, TAs>(IEnumerable<TType> types)
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

        public Container RegisterTypes<TType>(params TType[] types)
            where TType : Type
        {
            foreach (var type in types)
                Builder
                    .RegisterType(type)
                    .InstancePerLifetimeScope();
            return this;
        }

        public Container RegisterTypes<TType, TAs>(params TType[] types)
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

        public Container RegisterServices<TService>(IEnumerable<TService> services, IResolveMiddleware middleware, MiddlewareInsertionMode insertionMode = MiddlewareInsertionMode.EndOfPhase)
            where TService : Service
        {
            foreach (var service in services)
                Builder.RegisterServiceMiddleware(service, middleware, insertionMode);
            return this;
        }
        #endregion

        //public void Run() { }
    }
}