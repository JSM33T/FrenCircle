/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { Component, ElementRef, NgZone, ViewChild } from '@angular/core';
import { environment } from '../environments/environment';
import { filter, Observable } from 'rxjs';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { SidepanelComponent } from './components/shared/sidepanel/sidepanel.component';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { BackToTopComponent } from './components/shared/back-to-top/back-to-top.component';
import { AOSService } from './services/Animator/aos.service';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { ModalService } from './services/DOMServices/modal.service';
import { ApiHandlerService } from './services/Api/api-handler.service';
import { OffCanvasService } from './services/DOMServices/off-canvas.service';
@Component({
    selector: 'app-root',
    imports: [
        RouterOutlet,
        NavbarComponent,
        LoadingBarRouterModule,
        SidepanelComponent,
        BackToTopComponent,
    ],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
})
export class AppComponent {
    isLoading = true;
    loaderTime: number = environment.featureToggle.loaderTime;
    data: any;

    @ViewChild('aosElement', { static: true }) aosElement!: ElementRef;

    constructor(
        private router: Router,
        private aosService: AOSService,
        private zone: NgZone,
        private mdlService: ModalService,
        private offcanvasService: OffCanvasService,
        private apiHandler: ApiHandlerService,
    ) {
        this.router.events
            .pipe(filter((event) => event instanceof NavigationEnd))
            .subscribe((event: NavigationEnd) => {
                window.scrollTo({
                    top: 0,
                    behavior: 'smooth',
                });
                this.mdlService.closeAllModals();
                this.offcanvasService.closeAllOffcanvases();
            });

        //this.shouldRenderNavbar();
    }

    async ngOnInit() {
        //1. Rmove the main loader
        this.remLoader();

        //this.showToast();

        this.aosService.initAos(this.aosElement);

        //this.chackApiStatus();
    }

    remLoader() {
        setTimeout(() => {
            this.isLoading = false;
        }, this.loaderTime);
    }

    chackApiStatus() {
        this.apiHandler.get<any>('api/server').subscribe({
            next: (response) => {
                this.data = response;
                console.log('=================================');
                console.log(response);
                console.log('=================================');
                this.mdlService.apiToaster(response);
            },
            error: (error) => {
                console.error('Error fetching data:', error);
            },
        });
    }
}
