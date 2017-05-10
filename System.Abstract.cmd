@echo off
rem pushd src & System.Abstract.cmd & popd
rd /S/Q src.eventsources\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicebuses\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicecaches\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicelocators\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicelogs\packages\System.Abstract.1.0.0 2> nul
rd /S/Q src.servicemaps\packages\System.Abstract.1.0.0 2> nul

pushd src.eventsources & cmd /C EventSources.cmd & popd
pushd src.servicebuses & cmd /C ServiceBuses.cmd & popd
pushd src.servicecaches & cmd /C ServiceCaches.cmd & popd
pushd src.servicelocators & cmd /C ServiceLocators.cmd & popd
pushd src.servicelogs & cmd /C ServiceLogs.cmd & popd
pushd src.servicemaps & cmd /C ServiceMaps.cmd & popd
