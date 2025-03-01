import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./sections/home/home.component').then(
                (m) => m.HomeComponent,
            ),
    },
    {
        path: 'about',
        loadComponent: () =>
            import('./sections/about/about.component').then(
                (m) => m.AboutComponent,
            ),
    },
    {
        path: 'about/faq',
        loadComponent: () =>
            import('./sections/about/faq/faq.component').then(
                (m) => m.FaqComponent,
            ),
    },
    {
        path: 'about/contact',
        loadComponent: () =>
            import('./sections/about/contact/contact.component').then(
                (m) => m.ContactComponent,
            ),
    },
    {
        path: 'about/attributions',
        loadComponent: () =>
            import('./sections/about/attributions/attributions.component').then(
                (m) => m.AttributionsComponent,
            ),
    },
    {
        path: 'account',
        loadComponent: () =>
            import('./sections/account/account.component').then(
                (m) => m.AccountComponent,
            ),
    },
    {
        path: 'account/login',
        loadComponent: () =>
            import('./sections/account/login/login.component').then(
                (m) => m.LoginComponent,
            ),
    },
    {
        path: 'account/signup',
        loadComponent: () =>
            import('./sections/account/signup/signup.component').then(
                (m) => m.SignupComponent,
            ),
    },
    {
        path: 'account/verify',
        loadComponent: () =>
            import('./sections/account/verify/verify.component').then(
                (m) => m.VerifyComponent,
            ),
    },
    {
        path: 'studio/music',
        loadComponent: () =>
            import('./sections/studio/music/music.component').then(
                (m) => m.MusicComponent,
            ),
    },
    {
        path: 'account/profile',
        loadComponent: () =>
            import(
                './sections/account/profile/overview/overview.component'
            ).then((m) => m.OverviewComponent),
    },
    {
        path: 'blogs',
        loadComponent: () =>
            import('./sections/blog/browse/browse.component').then(
                (m) => m.BrowseComponent,
            ),
    },
];
