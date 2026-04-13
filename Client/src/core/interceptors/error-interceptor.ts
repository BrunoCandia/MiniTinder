import { HttpInterceptorFn } from '@angular/common/http';
import { throwError } from 'rxjs/internal/observable/throwError';
import { catchError } from 'rxjs/internal/operators/catchError';
import { ToastService } from '../services/toast-service';
import { inject } from '@angular/core/primitives/di';
import { NavigationExtras, Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const toastService = inject(ToastService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error) => {

      if(error) {
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              const modelStateErrors = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) {
                  modelStateErrors.push(error.error.errors[key]);
                }
              }

              throw modelStateErrors.flat();

            } else {
              toastService.showError(error.error);
            }            
            break;
          case 401:
            toastService.showError('Unauthorized');
            break;
          case 404:
            router.navigate(['/not-found']);
            //toastService.showError('Not Found');
            break;
          case 500:
            const navigationExtras: NavigationExtras = { state: { error: error.error } };
            router.navigateByUrl('/server-error', navigationExtras);

            //toastService.showError('Internal Server Error');
            break;
          default:
            toastService.showError('An error occurred');
            break;
        }
      }

      return throwError(() => error);
    })
  );  
};
