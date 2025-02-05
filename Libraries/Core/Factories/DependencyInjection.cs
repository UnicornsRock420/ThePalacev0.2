using Autofac;

namespace ThePalace.Core.Factories
{
    public class DependencyInjection
    {
        public DependencyInjection()
        {
            builder = new ContainerBuilder();
        }

        private ContainerBuilder builder;

        public DependencyInjection RegisterInstance<TInstance>(TInstance instance)
            where TInstance : Type
        {
            builder.RegisterInstance(instance);

            return this;
        }

        public DependencyInjection RegisterModule<TModule>(TModule module)
            where TModule : Module
        {
            builder.RegisterModule(module);

            return this;
        }

        public DependencyInjection RegisterType<TType>(TType type)
            where TType : Type
        {
            builder.RegisterType(type);

            return this;
        }
    }
}