@echo off
rem pushd src & System.Abstract.cmd & popd
rd /S/Q src.eventsources\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicebuses\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicecaches\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicelocators\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicelogs\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicemaps\packages\System.Abstract.1.0.0 2> nul

pushd src.eventsources & cmd EventSources.cmd & popd
pushd src.servicebuses & cmd ServiceBuses.cmd & popd
pushd src.servicecaches & cmd ServiceCaches.cmd & popd
pushd src.servicelocators & cmd ServiceLocators.cmd & popd
pushd src.servicelogs & cmd ServiceLogs.cmd & popd
pushd src.servicemaps & cmd ServiceMaps.cmd & popd
