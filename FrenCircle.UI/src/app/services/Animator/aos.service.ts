/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable, ElementRef, Injector } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import AOS from 'aos';
import { filter } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AOSService {
    constructor(
        private injector: Injector,
        private router: Router,
    ) {
        this.router.events
            .pipe(filter((event) => event instanceof NavigationEnd))
            .subscribe(() => {
                this.refreshAos();
            });
    }

    initAos(element?: ElementRef): void {
        AOS.init({
            disable: false,
            startEvent: 'DOMContentLoaded',
        });
    }

    refreshAos(): void {
        AOS.refresh();
    }

    destroyAos(): void {
        AOS.refreshHard();
    }
}
