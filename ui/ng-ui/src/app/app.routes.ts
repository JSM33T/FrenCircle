import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: '',
		loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent)
	},
	{
		path: 'account',
		loadComponent: () => import('./pages/account/account.component').then(m => m.AccountComponent)
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
