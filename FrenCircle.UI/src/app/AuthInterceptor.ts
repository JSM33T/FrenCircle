/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent,
} from '@angular/common/http';
import {
    Observable,
    throwError,
    switchMap,
    catchError,
    BehaviorSubject,
    filter,
    take,
} from 'rxjs';
import { ApiHandlerService } from './services/Api/api-handler.service';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {
    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<string | null> =
        new BehaviorSubject<string | null>(null);

    constructor(
        private apiHandler: ApiHandlerService,
        private router: Router,
    ) {}

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler,
    ): Observable<HttpEvent<any>> {
        let token = localStorage.getItem('token');

        const authReq = token
            ? req.clone({
                  headers: req.headers.set('Authorization', `Bearer ${token}`),
              })
            : req;

        return next.handle(authReq).pipe(
            catchError((error) => {
                if (error.status === 401 && !this.isRefreshing) {
                    this.isRefreshing = true;
                    this.refreshTokenSubject.next(null);

                    return this.apiHandler.refreshToken().pipe(
                        switchMap((response) => {
                            if (response.status === 200) {
                                token = response.data.token;
                                localStorage.setItem('token', token);
                                this.isRefreshing = false;
                                this.refreshTokenSubject.next(token);

                                // Retry the original request with the new token
                                const retryReq = req.clone({
                                    headers: req.headers.set(
                                        'Authorization',
                                        `Bearer ${token}`,
                                    ),
                                });
                                return next.handle(retryReq);
                            } else {
                                // If refresh fails, clear tokens and redirect to login
                                this.isRefreshing = false;
                                localStorage.removeItem('token');
                                localStorage.removeItem('refreshToken');
                                this.router.navigate(['/login']);
                                return throwError(
                                    () =>
                                        new Error(
                                            'Session expired. Please log in again.',
                                        ),
                                );
                            }
                        }),
                        catchError((err) => {
                            this.isRefreshing = false;
                            localStorage.removeItem('token');
                            localStorage.removeItem('refreshToken');
                            this.router.navigate(['account/login']);
                            return throwError(() => err);
                        }),
                    );
                } else if (error.status === 401) {
                    return this.refreshTokenSubject.pipe(
                        filter((token) => token != null),
                        take(1),
                        switchMap(() =>
                            next.handle(
                                req.clone({
                                    headers: req.headers.set(
                                        'Authorization',
                                        `Bearer ${this.refreshTokenSubject.value}`,
                                    ),
                                }),
                            ),
                        ),
                    );
                }

                return throwError(() => error);
            }),
        );
    }
}
