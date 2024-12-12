import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: '',
		loadComponent: () => import('./sections/home/home.component').then((m) => m.HomeComponent),
	},
	{
		path: 'about',
		loadComponent: () => import('./sections/about/about.component').then((m) => m.AboutComponent),
	},
	{
		path: 'contact',
		loadComponent: () => import('./sections/contact/contact.component').then((m) => m.ContactComponent),
	},
    {
		path: 'faq',
		loadComponent: () => import('./sections/faq/faq.component').then((m) => m.FaqComponent),
	},
    {
		path: 'account/login',
		loadComponent: () => import('./sections/account/login/login.component').then((m) => m.LoginComponent),
	}
];
