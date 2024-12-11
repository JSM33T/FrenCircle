import { Routes } from '@angular/router';

export const STUDIO_ROUTES: Routes = [
	{
		path: '',
		loadComponent: () => import('./studiohome/studiohome.component').then((m) => m.StudiohomeComponent),
	},
    {
		path: 'music',
		loadComponent: () => import('./music/music.component').then((m) => m.MusicComponent),
	},
    {
		path: 'apps',
		loadComponent: () => import('./apps/apps.component').then((m) => m.AppsComponent),
	}
];
