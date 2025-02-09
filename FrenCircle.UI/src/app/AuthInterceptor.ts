/* eslint-disable @typescript-eslint/no-explicit-any */
// /* eslint-disable @typescript-eslint/no-explicit-any */
// import { Injectable } from '@angular/core';
// import {
//     HttpInterceptor,
//     HttpRequest,
//     HttpHandler,
//     HttpEvent,
// } from '@angular/common/http';
// import { Observable } from 'rxjs';

// @Injectable({ providedIn: 'root' })
// export class AuthInterceptor implements HttpInterceptor {
//     intercept(
//         req: HttpRequest<any>,
//         next: HttpHandler,
//     ): Observable<HttpEvent<any>> {
//         const token = localStorage.getItem('token');
//         const authReq = token
//             ? req.clone({
//                   headers: req.headers.set('Authorization', `Bearer ${token}`),
//               })
//             : req;
//         return next.handle(authReq);
//     }
// }
import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError, switchMap, catchError } from 'rxjs';
import { ApiHandlerService } from './services/Api/api-handler.service';

@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {
    private isRefreshing = false;

    constructor(private apiHandler: ApiHandlerService) {}

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler,
    ): Observable<HttpEvent<any>> {
        const token = localStorage.getItem('token');
        const authReq = token
            ? req.clone({
                  headers: req.headers.set('Authorization', `Bearer ${token}`),
              })
            : req;
        console.log('token' + token);

        return next.handle(authReq).pipe(
            catchError((error) => {
                if (
                    error instanceof HttpErrorResponse &&
                    error.status === 409 &&
                    !this.isRefreshing
                ) {
                    this.isRefreshing = true;
                    return this.apiHandler.refreshToken().pipe(
                        switchMap((response) => {
                            localStorage.setItem('token', response.data.token);
                            this.isRefreshing = false;
                            return next.handle(
                                req.clone({
                                    headers: req.headers.set(
                                        'Authorization',
                                        `Bearer ${response.data.token}`,
                                    ),
                                }),
                            );
                        }),
                        catchError((err) => {
                            this.isRefreshing = false;
                            return throwError(() => err);
                        }),
                    );
                }
                return throwError(() => error);
            }),
        );
    }
}
