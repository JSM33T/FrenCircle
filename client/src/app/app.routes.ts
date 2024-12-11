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
		path: 'about',
		loadComponent: () => import('./sections/studio/studiohome/studiohome.component').then((m) => m.StudiohomeComponent),
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
	},
    {
		path: 'gallery',
		loadChildren: () => import('./sections/gallery/gallery.routes').then((m) => m.GALLERY_ROUTES),
	},
	{
		path: 'studio',
		loadChildren: () => import('./sections/studio/studio.routes').then((m) => m.STUDIO_ROUTES),
	},
    {
		path: 'blogs',
		loadChildren: () => import('./sections/blogs/blog.routes').then((m) => m.BLOG_ROUTES),
	}
];
