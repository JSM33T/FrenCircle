import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';

import { GoogleLoginProvider, SocialAuthServiceConfig } from '@abacritt/angularx-social-login';

export const appConfig: ApplicationConfig = {
	providers: [provideHttpClient(), provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),{
        provide: 'SocialAuthServiceConfig',
        useValue: {
            autoLogin: false,
            providers: [
                {
                    id: GoogleLoginProvider.PROVIDER_ID,
                    provider: new GoogleLoginProvider('881148390473-8ie9hsg4a04qobhalnpfp8ibo5h3cn6n.apps.googleusercontent.com')
                }
            ]
        } as SocialAuthServiceConfig
    }],
};
