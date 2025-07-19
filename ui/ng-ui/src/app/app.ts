import { Component, OnInit, AfterViewInit, Renderer2, Inject, PLATFORM_ID } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { CommonModule, DOCUMENT, isPlatformBrowser, isPlatformServer } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatSidenavModule,
    MatListModule,
    MatSlideToggleModule,
    CommonModule
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App implements OnInit, AfterViewInit {
  protected title = 'FrenCircle';
  isHandset = false;
  isDarkMode = false;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.breakpointObserver.observe([Breakpoints.Handset])
      .subscribe(result => {
        this.isHandset = result.matches;
      });
  }

  ngOnInit() {
    // SSR: default to light theme; Browser: from localStorage/cookie
    let theme: string | null = null;
    if (isPlatformBrowser(this.platformId)) {
      // Browser: read from localStorage first, then cookie
      theme = localStorage.getItem('theme');
      if (!theme) {
        try {
          const cookies = document.cookie || '';
          const match = cookies.match(/(?:^|; )theme=([^;]*)/);
          theme = match ? decodeURIComponent(match[1]) : null;
        } catch (e) {
          // Fallback if cookie access fails
          theme = null;
        }
      }
    }
    // On SSR, default to light theme (will be corrected on client hydration)
    this.isDarkMode = theme === 'dark';
    this.applyTheme();
  }

  ngAfterViewInit() {
    // Ensure theme is applied after view is fully initialized
    if (isPlatformBrowser(this.platformId)) {
      setTimeout(() => {
        this.applyTheme();
      }, 0);
    }
  }

  toggleDarkMode() {
    this.isDarkMode = !this.isDarkMode;
    this.applyTheme();
    const themeValue = this.isDarkMode ? 'dark' : 'light';
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('theme', themeValue);
      // Set cookie for persistence (expires in 1 year)
      try {
        document.cookie = `theme=${themeValue};path=/;max-age=31536000;SameSite=Lax`;
      } catch (e) {
        // Cookie setting failed, but localStorage should work
        console.warn('Could not set theme cookie:', e);
      }
    }
  }

  private applyTheme() {
    if (isPlatformBrowser(this.platformId)) {
      const body = this.document.body;
      // Force removal first to ensure clean state
      this.renderer.removeClass(body, 'dark-theme');
      // Add class if dark mode is enabled
      if (this.isDarkMode) {
        this.renderer.addClass(body, 'dark-theme');
      }
      // Debug log to verify the class is being applied
      console.log('Theme applied:', this.isDarkMode ? 'dark' : 'light', 'Body classes:', body.className);
    }
  }
}
