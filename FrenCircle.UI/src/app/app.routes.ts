import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path:"",
        loadComponent: () => import('./components/home/home.component').then((m) => m.HomeComponent),
    },
    {
        path:"about",
        loadComponent: () => import('./components/about/about.component').then((m) => m.AboutComponent),
    }
];
