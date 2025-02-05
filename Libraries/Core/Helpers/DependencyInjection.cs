using Autofac;

namespace ThePalace.Core.Helpers
{
    public class DependencyInjection
    {
        public DependencyInjection()
        {
            builder = new ContainerBuilder();
        }

        private ContainerBuilder builder;

        #region Register Methods
        public DependencyInjection RegisterInstance<TInstance>(IEnumerable<TInstance> instances)
            where TInstance : Type
        {
            builder.RegisterInstance(instances);

            return this;
        }

        public DependencyInjection RegisterInstance<TInstance>(params TInstance[] instances)
            where TInstance : Type
        {
            builder.RegisterInstance(instances);

            return this;
        }

        public DependencyInjection RegisterModule<TModule>(IEnumerable<TModule> modules)
            where TModule : Module
        {
            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            return this;
        }

        public DependencyInjection RegisterModule<TModule>(params TModule[] modules)
            where TModule : Module
        {
            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            return this;
        }

        public DependencyInjection RegisterType<TType>(IEnumerable<TType> types)
            where TType : Type
        {
            foreach (var type in types)
            {
                builder.RegisterType(type);
            }

            return this;
        }

        public DependencyInjection RegisterType<TType>(params TType[] types)
            where TType : Type
        {
            foreach (var type in types)
            {
                builder.RegisterType(type);
            }

            return this;
        }
        #endregion
    }
}