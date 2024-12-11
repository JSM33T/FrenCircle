import { Routes } from '@angular/router';

export const BLOG_ROUTES: Routes = [
	{
		path: '',
		loadComponent: () => import('./bloghome/bloghome.component').then((m) => m.BloghomeComponent),
	}
];
