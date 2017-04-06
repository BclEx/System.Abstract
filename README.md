#System.Abstract#

We believe it's time to for the base common library (bcl) to start extending into standard patterns. Previously iserviceprovider was a machinicsm to handle all use cases. 

But now some well known use cases have been established. Caching, logging, service locators, etc.

Organically some packages have established them selfs to make these abstractions. Like common.logger or xyzserviceloctor. But because they are in isolation their use is kludgy. 
•	Common logger of two different versions in dependent packages, and your library uses nlog.
•	Because there is no standard, a library like rhinobus will make its own abstraction for service locators. But your dependent app is using its own service locator. So somehow you need to inject into both containers, or slipstream your container into rinhobus's abstraction. 

Some design tenants should be held
•	Abstraction only handles the common core features of the design system
•	The abstraction provider interface must still be accessible.
•	Non handled features of the specific providers features would be handled by accessing the base provider.

We also feel a manager pattern is the proper way of accessing and configuring these providers. 
•	It must also support multiple instances of a service
•	Must have a default singleton instance for simplicity
•	It must be lazy loaded to allow possible configuration with out ramp up times
•	It must handle dependency chains
•	It must provide optional logging for diagnostics.

We feel the common core services which should be represented first are as follows.

•	IEventSource and EventSourceManager
•	IServiceBus and EventServiceBusManager
•	IServiceCache and ServiceCacheManager
•	IServiceLocator and ServiceLocatorManager
•	IServiceLog and ServiceLogManager
•	IServiceMap and ServiceMapManager
