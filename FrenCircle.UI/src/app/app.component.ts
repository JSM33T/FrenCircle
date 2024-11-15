import { Component, OnInit } from '@angular/core';
import {
  NavigationEnd,
  Router,
  RouterModule,
} from '@angular/router';
import { filter } from 'rxjs';
import { NgClass } from '@angular/common';
import { NavbarComponent } from "./components/shared/navbar/navbar.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, NgClass, NavbarComponent],
  templateUrl:  './app.component.html'
})
export class AppComponent implements OnInit {
  constructor(private router: Router) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        if (typeof window !== 'undefined') {
          window.scrollTo({
            top: 0,
            behavior: 'smooth',
          });
        }
        this.isHidden = true;
        console.log('route changed');
      });
  }

  isHidden: boolean = true;

  ngOnInit(): void {}

  toggleNavMenu() {
    this.isHidden = !this.isHidden;
  }
}
