import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterModule, RouterOutlet } from '@angular/router';
import { NavbarComponent } from "./components/shared/navbar/navbar.component";
import { filter } from 'rxjs';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [RouterOutlet, NavbarComponent, RouterModule],
	templateUrl: './app.component.html',
	styleUrl: './app.component.css'
})
export class AppComponent {
	constructor(private router: Router,) {
		this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
			window.scrollTo({
				top: 0,
				behavior: 'smooth',
			});
			console.log("route changed");
		});
	}
}
