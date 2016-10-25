﻿using ControlWorks.Services.Configuration;
using ControlWorks.Services.Pvi;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace ControlWorks.Services.Business
{
    public interface ITypeRepository
    {
        T GetInstance<T>();
    }

    public class TypeRepository : ITypeRepository
    {
        private UnityServiceLocator _locator;

        public TypeRepository()
        {
            _locator = new UnityServiceLocator(ConfigureUnityContainer());
            ServiceLocator.SetLocatorProvider(() => _locator);
        }

        private IUnityContainer ConfigureUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IRequestProcessor, RequestProcessor>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPviApplication, PviApplication>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPviProcessor, PviProcessor>(new ContainerControlledLifetimeManager());
            container.RegisterType<IConfigurationService, ConfigurationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IConfigurationProcessor, ConfigurationProcessor>(new ContainerControlledLifetimeManager());
            container.RegisterType<IVariableProcessor, VariableProcessor>(new ContainerControlledLifetimeManager());
            container.RegisterType<IVariableApi, VariableApi>(new ContainerControlledLifetimeManager());

            return container;
        }

        public T GetInstance<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }


    }
}
