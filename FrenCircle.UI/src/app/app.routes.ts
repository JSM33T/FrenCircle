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
];
