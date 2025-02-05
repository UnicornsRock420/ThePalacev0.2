using Autofac;

namespace ThePalace.Core.Entities.DependencyInjection
{
    public partial class Container
    {
        public Container() =>
            builder = new ContainerBuilder();

        private ContainerBuilder builder;

        #region Register Methods
        public Container RegisterInstances<TInstance>(IEnumerable<TInstance> instances)
            where TInstance : Type
        {
            builder.RegisterInstance(instances);
            return this;
        }

        public Container RegisterInstances<TInstance>(params TInstance[] instances)
            where TInstance : Type
        {
            builder.RegisterInstance(instances);
            return this;
        }

        public Container RegisterModules<TModule>(IEnumerable<TModule> modules)
            where TModule : Module
        {
            foreach (var module in modules)
                builder.RegisterModule(module);
            return this;
        }

        public Container RegisterModules<TModule>(params TModule[] modules)
            where TModule : Module
        {
            foreach (var module in modules)
                builder.RegisterModule(module);
            return this;
        }

        public Container RegisterTypes<TType>(IEnumerable<TType> types)
            where TType : Type
        {
            foreach (var type in types)
                builder.RegisterType(type);
            return this;
        }

        public Container RegisterTypes<TType>(params TType[] types)
            where TType : Type
        {
            foreach (var type in types)
                builder.RegisterType(type);
            return this;
        }
        #endregion
    }
}