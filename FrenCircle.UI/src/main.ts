/* eslint-disable @typescript-eslint/no-explicit-any */
// Create a function adapter for your interceptor
import { inject } from '@angular/core';
import {
    HttpHandler,
    HttpInterceptorFn,
    HttpRequest,
    provideHttpClient,
    withInterceptors,
} from '@angular/common/http';
import { AuthInterceptor } from './app/AuthInterceptor';
import { appConfig } from './app/app.config';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';

export const authInterceptorFn: HttpInterceptorFn = (
    req: HttpRequest<any>,
    next,
) => {
    const interceptor = inject(AuthInterceptor);
    const nextHandler: HttpHandler = { handle: next };
    return interceptor.intercept(req, nextHandler);
};

bootstrapApplication(AppComponent, {
    providers: [
        ...appConfig.providers,
        provideHttpClient(withInterceptors([authInterceptorFn])),
        AuthInterceptor,
    ],
}).catch((err) => console.error(err));
