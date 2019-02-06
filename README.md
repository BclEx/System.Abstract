# System.Abstract #

We believe it is time for the base common library (`bcl`) to start extending into some well-established service patterns.

Previously `IServiceProvider` was the only provided mechanism to handle all service retievals. With its only method `GetService` this interface is able to handle any service. However some well-established service patterns have emerged; caching, logging, service locators, etc.

Organically some packages, like `common.logger` or `CommonServiceLocator` have established themselves as the primary service abstraction. But because they developed in isolation, and with no common pattern or design tenants their use can become kludgy. 

Kludgy situations:
* Common logger of two different versions in dependent packages, and your library uses nlog.
* Because there is no standard, a library like `Rhino.ServiceBus` will implement its own abstraction for service locators. isolating your app container, unless you know how to slipstream your container into the `Rhino.ServiceBus` abstraction. 

Some design tenants should be held:
* Abstractions only handle the most common core features of the design system.
* The abstraction provider must still be accessible.
* Unhandled features of the specific providers features would be handled by accessing the base provider.
* A single namespace should provide contracts and extensions

We also feel a manager pattern is the proper way of accessing and configuring these providers. 
* It must also support multiple instances of a service
* Must have a default singleton instance for simplicity
* It must be lazy loaded to allow possible configuration with out ramp up times
* It must handle dependency chains
* It must provide optional logging for diagnostics.

We feel the common core services which should be represented first are as follows.

* Event Sourcing - `IEventSource` and `EventSourceManager`
* Enterprise Service Bus - `IServiceBus` and EventServiceBusManager`
* Cachine - `IServiceCache` and `ServiceCacheManager`
* Service Locator - `IServiceLocator` and `ServiceLocatorManager`
* Logging - `IServiceLog` and `ServiceLogManager`
* Mapping - `IServiceMap` and `ServiceMapManager`
