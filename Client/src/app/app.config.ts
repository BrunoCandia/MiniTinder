import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { InitializationService } from '../core/services/initialization-service';
import { lastValueFrom } from 'rxjs';
import { errorInterceptor } from '../core/interceptors/error-interceptor';
import { jwtInterceptorInterceptor } from '../core/interceptors/jwt-interceptor-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes, withViewTransitions()),
    provideHttpClient(withInterceptors([errorInterceptor, jwtInterceptorInterceptor])),
    provideAppInitializer(async () => {
      const initializationService = inject(InitializationService);

      return new Promise<void>(async (resolve) => {
        setTimeout(async () => {
          try {
            await lastValueFrom(initializationService.initialization());
          } finally {
            console.log('Initialization completed');
            const spalsh = document.getElementById('initial-splash');
            if (spalsh) {
              spalsh.remove();
            }
            resolve();
          }
      }, 500);

      // try {
      //   return lastValueFrom(initializationService.initialization());
      // } finally {
      //   console.log('Initialization completed');
      //   const spalsh = document.getElementById('initial-splash');
      //   if (spalsh) {
      //     spalsh.remove();
      //   }
      // }
    })
  })]
};
