import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterModule, RouterOutlet } from '@angular/router';
import { NavbarComponent } from "./components/shared/navbar/navbar.component";
import { filter } from 'rxjs';
import { BackToTopComponent } from "./components/shared/back-to-top/back-to-top.component";

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [RouterOutlet, NavbarComponent, RouterModule, BackToTopComponent],
	template: `
		<app-navbar></app-navbar>
		<router-outlet></router-outlet>
		<app-back-to-top></app-back-to-top>
	`
})
export class AppComponent {
	constructor(private router: Router,) {
		this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
			if (typeof window !== 'undefined') {
				window.scrollTo({
					top: 0,
					behavior: 'smooth',
				});
			}
			console.log("route changed");
		});
	}
}
