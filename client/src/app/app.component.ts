import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { environment } from '../environments/environment';
import { filter } from 'rxjs';

import { BackToTopComponent } from './components/ui/back-to-top/back-to-top.component';
import { AnimatorService } from './services/animator/animator.service';
import { ThemeEngineService } from './services/themeEngine/themeengine.service';
import { SidepanelComponent } from './components/shared/sidepanel/sidepanel.component';
import { BootstrapService } from './services/DOMServices/bootstrap.service';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [RouterOutlet, NavbarComponent, BackToTopComponent, SidepanelComponent,LoadingBarRouterModule],
	templateUrl: './app.component.html',
	styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
	//Meta

	title = "jassi's web space";

	themes = [];

	//Props
	isLoading: boolean = true;
	loaderTime: number = environment.featureToggle.loaderTime;

	@ViewChild('aosElement', { static: true }) aosElement!: ElementRef;

	constructor(
		private router: Router,
		private aosService: AnimatorService,
		private dynamicStyleService: ThemeEngineService,
		private zone: NgZone,
        private bsService : BootstrapService
	) {
		this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
			window.scrollTo({
				top: 0,
				behavior: 'smooth',
			});
            this.closeAllModals();
		});
		
		this.shouldRenderNavbar();
	}

	ngOnInit(): void {
		this.remLoader(), this.aosService.initAos(this.aosElement);

		this.dynamicStyleService.initheme();
	}

	remLoader() {
		setTimeout(() => {
			this.isLoading = false;
		}, this.loaderTime);
	}

	shouldRenderNavbar(): boolean {
		return !this.router.url.includes('account/');
	}

	closeAllModals() {
        this.bsService.closeAllModals();
    }
}
