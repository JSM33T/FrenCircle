import { Injectable, ElementRef, Injector } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import Aos from 'aos';
import AOS from 'aos';
import { filter } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AnimatorService {
  constructor(private injector: Injector, private router: Router) {
    this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(() => {
      this.refreshAos();
    });
  }

  initAos(element?: ElementRef): void {
    AOS.init({
      disable: false,
      startEvent: 'DOMContentLoaded'
    });
  }

  refreshAos(): void {
    AOS.refresh();
  }

  destroyAos(): void {
    AOS.refreshHard();
  }
}
