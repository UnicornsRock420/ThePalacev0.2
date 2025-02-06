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
        public Container RegisterInstances<TInstance>(IEnumerable<TInstance> instances)
            where TInstance : Type
        {
            Builder.RegisterInstance(instances);
            return this;
        }

        public Container RegisterInstances<TInstance>(params TInstance[] instances)
            where TInstance : Type
        {
            Builder.RegisterInstance(instances);
            return this;
        }

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

        public Container RegisterTypes<TType>(IEnumerable<TType> types)
            where TType : Type
        {
            foreach (var type in types)
                Builder.RegisterType(type);
            return this;
        }

        public Container RegisterTypes<TType>(params TType[] types)
            where TType : Type
        {
            foreach (var type in types)
                Builder.RegisterType(type);
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