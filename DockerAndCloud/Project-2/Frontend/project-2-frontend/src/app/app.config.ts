import { ApplicationConfig, inject, Inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { ApiConfigService } from './api.config.service';
import { ApiEndpointsService } from './api.endpoints.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideAppInitializer(()=>{
      const appConfigService = inject(ApiConfigService);
      const apiEndpointsService = inject(ApiEndpointsService);
      return appConfigService.load().then(res => apiEndpointsService.init());
    })
  ]
};
