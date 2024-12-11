import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-theme-toggle',
  standalone: true,
  imports: [],
  templateUrl: './theme-toggle.component.html',
  styleUrl: './theme-toggle.component.css'
})
export class ThemeToggleComponent implements OnInit {
  isDarkMode = false;

  ngOnInit(): void {
    this.isDarkMode = this.getStoredTheme() === 'dark';
    this.updateTheme(this.isDarkMode);
  }

  toggleTheme(): void {
    this.isDarkMode = !this.isDarkMode;
    this.updateStoredTheme();
    this.updateTheme(this.isDarkMode);
  }

  private getStoredTheme(): string {
    return localStorage.getItem('theme') || 'light';
  }

  private updateStoredTheme(): void {
    const theme = this.isDarkMode ? 'dark' : 'light';
    localStorage.setItem('theme', theme);
  }

  private updateTheme(isDarkMode: boolean): void {
    document.documentElement.setAttribute('data-bs-theme', isDarkMode ? 'dark' : 'light');
  }
}
