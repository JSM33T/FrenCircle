import { Injectable, effect, signal, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export type ThemeMode = 'light' | 'dark' | 'auto';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly THEME_KEY = 'frencircle-theme-preference';
  private readonly platformId = inject(PLATFORM_ID);
  private readonly isBrowser = isPlatformBrowser(this.platformId);

  // Signal for current theme mode
  private themeMode = signal<ThemeMode>('light');

  // Signal for actual applied theme (resolved from auto)
  private appliedTheme = signal<'light' | 'dark'>('light');

  // Public getters
  currentThemeMode = this.themeMode.asReadonly();
  currentAppliedTheme = this.appliedTheme.asReadonly();

  constructor() {
    // Only initialize theme-related functionality in the browser
    if (this.isBrowser) {
      // Initialize theme from localStorage or default to light
      this.initializeTheme();

      // Set up media query listener for auto theme
      this.setupMediaQueryListener();

      // Effect to apply theme changes to the DOM
      effect(() => {
        this.applyTheme(this.appliedTheme());
      });
    }
  }

  /**
   * Set the theme mode
   */
  setThemeMode(mode: ThemeMode): void {
    this.themeMode.set(mode);
    if (this.isBrowser) {
      localStorage.setItem(this.THEME_KEY, mode);
    }
    this.updateAppliedTheme();
  }

  /**
   * Toggle between light and dark mode
   */
  toggleTheme(): void {
    const current = this.themeMode();
    if (current === 'auto') {
      // If auto, switch to opposite of current applied theme
      this.setThemeMode(this.appliedTheme() === 'light' ? 'dark' : 'light');
    } else {
      // Toggle between light and dark
      this.setThemeMode(current === 'light' ? 'dark' : 'light');
    }
  }

  /**
   * Get the appropriate icon for current theme state
   */
  getThemeIcon(): string {
    const mode = this.themeMode();
    const applied = this.appliedTheme();

    if (mode === 'auto') {
      return applied === 'light' ? 'brightness_auto' : 'brightness_auto';
    }
    return applied === 'light' ? 'dark_mode' : 'light_mode';
  }

  /**
   * Get theme toggle tooltip text
   */
  getThemeTooltip(): string {
    const mode = this.themeMode();
    const applied = this.appliedTheme();

    if (mode === 'auto') {
      return `Auto theme (currently ${applied})`;
    }
    return `Switch to ${applied === 'light' ? 'dark' : 'light'} mode`;
  }

  private initializeTheme(): void {
    if (!this.isBrowser) {
      return;
    }

    const savedTheme = localStorage.getItem(this.THEME_KEY) as ThemeMode;
    const validThemes: ThemeMode[] = ['light', 'dark', 'auto'];

    if (savedTheme && validThemes.includes(savedTheme)) {
      this.themeMode.set(savedTheme);
    }

    this.updateAppliedTheme();
  }

  private setupMediaQueryListener(): void {
    if (!this.isBrowser) {
      return;
    }

    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

    // Initial check
    this.updateAppliedTheme();

    // Listen for changes
    mediaQuery.addEventListener('change', () => {
      this.updateAppliedTheme();
    });
  }

  private updateAppliedTheme(): void {
    const mode = this.themeMode();

    if (mode === 'auto') {
      if (this.isBrowser) {
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        this.appliedTheme.set(prefersDark ? 'dark' : 'light');
      } else {
        // Default to light theme on server
        this.appliedTheme.set('light');
      }
    } else {
      this.appliedTheme.set(mode);
    }
  }

  private applyTheme(theme: 'light' | 'dark'): void {
    if (!this.isBrowser) {
      return;
    }

    const htmlElement = document.documentElement;

    // Temporarily disable transitions for initial theme application
    const isInitialLoad = !htmlElement.classList.contains('light-theme') && !htmlElement.classList.contains('dark-theme');
    if (isInitialLoad) {
      htmlElement.classList.add('no-transition');
    }

    // Remove existing theme classes
    htmlElement.classList.remove('light-theme', 'dark-theme');

    // Add new theme class
    htmlElement.classList.add(`${theme}-theme`);

    // Update color-scheme for better browser integration
    htmlElement.style.colorScheme = theme;

    // Re-enable transitions after theme is applied
    if (isInitialLoad) {
      // Use requestAnimationFrame to ensure the theme is applied before removing no-transition
      requestAnimationFrame(() => {
        htmlElement.classList.remove('no-transition');
      });
    }

    // Dispatch custom event for other components that might need to know
    window.dispatchEvent(new CustomEvent('theme-changed', {
      detail: { theme, mode: this.themeMode() }
    }));
  }
}
