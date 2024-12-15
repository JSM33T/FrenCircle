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
		path: 'showcase',
		loadComponent: () => import('./sections/showcase/showcase.component').then((m) => m.ShowcaseComponent),
	},
    {
		path: 'studio',
		loadComponent: () => import('./sections/studio/studio.component').then((m) => m.StudioComponent),
	},
    {
		path: 'wall',
		loadComponent: () => import('./sections/wall/wall.component').then((m) => m.WallComponent),
	},
    {
		path: 'account/login',
		loadComponent: () => import('./sections/account/login/login.component').then((m) => m.LoginComponent),
	}
];
