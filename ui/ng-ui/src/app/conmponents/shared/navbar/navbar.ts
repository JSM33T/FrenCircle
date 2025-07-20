import { Component, Input, OnInit, inject, effect } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AuthService } from '../../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatSidenavModule,
    MatListModule,
    CommonModule
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar implements OnInit {
  @Input() title: string = 'FrenCircle';
  isHandset = false;

  user: { name: string; avatar: string; loggedIn: boolean } = {
    name: 'Guest',
    avatar: 'person',
    loggedIn: false
  };

  private authService = inject(AuthService);

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
    this.breakpointObserver.observe([Breakpoints.Handset])
      .subscribe(result => {
        this.isHandset = result.matches;
      });
  }
}
