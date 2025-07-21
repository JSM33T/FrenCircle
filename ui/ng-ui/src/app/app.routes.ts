import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'account',
    children: [
      {
        path: '',
        loadComponent: () => import('./pages/account/login/login').then(m => m.Login)
      },
      {
        path: 'login',
        loadComponent: () => import('./pages/account/login/login').then(m => m.Login)
      },
      {
        path: 'register',
        loadComponent: () => import('./pages/account/signup/signup').then(m => m.Signup)
      }
    ]
  },
  {
    path: 'auth/callback',
    loadComponent: () => import('./pages/account/oauth-callback/oauth-callback.component').then(m => m.OAuthCallbackComponent)
  },
  {
    path: 'stream',
    loadComponent: () => import('./pages/stream/stream.component').then(m => m.StreamComponent)
  },
  {
    path: 'parties',
    loadComponent: () => import('./pages/parties/parties.component').then(m => m.PartiesComponent)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
