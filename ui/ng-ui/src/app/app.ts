import { Component, OnInit, AfterViewInit, Renderer2, Inject, PLATFORM_ID } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatIconRegistry } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { CommonModule, DOCUMENT, isPlatformBrowser } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';
import { Navbar } from './conmponents/shared/navbar/navbar';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MatSlideToggleModule,
    CommonModule,
    Navbar
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App implements OnInit, AfterViewInit {
  protected title = 'FrenCircle';
  isDarkMode = false;

  constructor(
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    @Inject(PLATFORM_ID) private platformId: Object,
    private matIconRegistry: MatIconRegistry,
    private domSanitizer: DomSanitizer
  ) {
    // Register Google icon
    this.matIconRegistry.addSvgIconLiteral(
      'google',
      this.domSanitizer.bypassSecurityTrustHtml(`
        <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
          <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
          <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"/>
          <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
        </svg>
      `)
    );
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
