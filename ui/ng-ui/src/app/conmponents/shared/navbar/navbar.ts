import { Component, Input, OnInit, inject, effect, ViewChild, OnDestroy } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule, MatSidenav } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AuthService } from '../../../services/auth.service';
import { ThemeService } from '../../../services/theme.service';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';

@Component({
	selector: 'app-navbar',
	imports: [
		RouterLink,
		RouterLinkActive,
		MatToolbarModule,
		MatButtonModule,
		MatIconModule,
		MatMenuModule,
		MatSidenavModule,
		MatListModule,
		MatDividerModule,
		MatTooltipModule,
		CommonModule
	],
	templateUrl: './navbar.html',
	styleUrl: './navbar.scss'
})
export class Navbar implements OnInit, OnDestroy {
	@Input() title: string = 'FrenCircle';
	@ViewChild('drawer') drawer!: MatSidenav;

	isHandset = false;
	private destroy$ = new Subject<void>();

	user: { name: string; avatar: string; loggedIn: boolean } = {
		name: 'Guest',
		avatar: 'person',
		loggedIn: false
	};

	private authService = inject(AuthService);
	protected themeService = inject(ThemeService);

	constructor(private breakpointObserver: BreakpointObserver) {
		// Reactively update user state using signals
		effect(() => {
			const isAuth = this.authService.isAuthenticated();
			if (isAuth) {
				const user = this.authService.currentUser();
				this.user = {
					name: user?.firstName || user?.username || user?.email || 'User',
					avatar: user?.avatar || 'person',
					loggedIn: true
				};
			} else {
				this.user = {
					name: 'Guest',
					avatar: 'person',
					loggedIn: false
				};
			}
		});
	}

	ngOnInit() {
		// Observe breakpoint changes with proper cleanup
		this.breakpointObserver.observe([Breakpoints.Handset])
			.pipe(takeUntil(this.destroy$))
			.subscribe(result => {
				this.isHandset = result.matches;
				// Close drawer when switching to desktop view
				if (!this.isHandset && this.drawer?.opened) {
					this.drawer.close();
				}
			});
	}

	ngOnDestroy() {
		this.destroy$.next();
		this.destroy$.complete();
	}

	/**
	 * Handles user logout with proper error handling
	 */
	onLogout(): void {
		try {
			this.authService.logout();
			// Close mobile drawer if open
			if (this.isHandset && this.drawer?.opened) {
				this.drawer.close();
			}
		} catch (error) {
			console.error('Logout failed:', error);
		}
	}

	/**
	 * Handles navigation and closes mobile drawer
	 */
	onNavigate(): void {
		if (this.isHandset && this.drawer?.opened) {
			this.drawer.close();
		}
	}

	/**
	 * Toggle theme mode
	 */
	onToggleTheme(): void {
		this.themeService.toggleTheme();
	}

	/**
	 * Get current theme icon
	 */
	get themeIcon(): string {
		return this.themeService.getThemeIcon();
	}

	/**
	 * Get theme toggle tooltip
	 */
	get themeTooltip(): string {
		return this.themeService.getThemeTooltip();
	}
}
