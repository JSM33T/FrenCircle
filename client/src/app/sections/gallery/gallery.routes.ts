import { Routes } from '@angular/router';

export const GALLERY_ROUTES: Routes = [
	{
		path: '',
		loadComponent: () => import('./galleryhome/galleryhome.component').then((m) => m.GalleryhomeComponent),
	}
];
