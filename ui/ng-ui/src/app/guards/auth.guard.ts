import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
	const authService = inject(AuthService);
	const router = inject(Router);

	if (authService.isAuthenticated()) {
		return true;
	}

	// Store the attempted URL for redirecting after login
	sessionStorage.setItem('returnUrl', state.url);

	// Redirect to login page
	router.navigate(['/account'], {
		queryParams: { returnUrl: state.url }
	});

	return false;
};

export const adminGuard: CanActivateFn = (route, state) => {
	const authService = inject(AuthService);
	const router = inject(Router);

	if (authService.isAuthenticated() && authService.isAdmin()) {
		return true;
	}

	if (!authService.isAuthenticated()) {
		sessionStorage.setItem('returnUrl', state.url);
		router.navigate(['/account'], {
			queryParams: { returnUrl: state.url }
		});
	} else {
		// User is authenticated but not admin
		router.navigate(['/']);
	}

	return false;
};
