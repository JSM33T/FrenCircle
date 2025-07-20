import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';

import { AuthService, LoginRequest } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDividerModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  // Reactive state
  readonly isLoading = signal(false);
  readonly hidePassword = signal(true);
  readonly showForgotPassword = signal(false);
  readonly isOAuthLoading = signal(false);

  loginForm: FormGroup;
  forgotPasswordForm: FormGroup;

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      rememberMe: [false]
    });

    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    // First, try to restore authentication state
    this.authService.restoreAuthState().subscribe({
      next: (isAuthenticated) => {
        if (isAuthenticated) {
          // User is already authenticated, redirect
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigate([returnUrl]);
          return;
        }

        // If not authenticated, handle OAuth callback if present
        this.handleRouteParams();
      },
      error: () => {
        // Authentication restoration failed, continue with login flow
        this.handleRouteParams();
      }
    });
  }

  private handleRouteParams(): void {
    this.route.queryParams.subscribe(params => {
      if (params['code']) {
        this.handleOAuthCallback(params['code'], params['state']);
      }
    });
  }

  async onLogin(): Promise<void> {
    if (this.loginForm.invalid) {
      this.markFormGroupTouched(this.loginForm);
      return;
    }

    this.isLoading.set(true);

    const credentials: LoginRequest = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password,
      rememberMe: this.loginForm.value.rememberMe
    };

    this.authService.login(credentials).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        this.snackBar.open('Login successful!', 'Close', { duration: 3000 });

        // Redirect to intended page or home
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigate([returnUrl]);
      },
      error: (error) => {
        this.isLoading.set(false);
        this.snackBar.open(error.message || 'Login failed. Please try again.', 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }

  async onGoogleLogin(): Promise<void> {
    try {
      this.isOAuthLoading.set(true);

      this.authService.getGoogleAuthUrl().subscribe({
        next: (response) => {
          // Redirect to Google OAuth
          window.location.href = response.authUrl;
        },
        error: (error) => {
          this.isOAuthLoading.set(false);
          this.snackBar.open('Failed to initiate Google login. Please try again.', 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
      });
    } catch (error) {
      this.isOAuthLoading.set(false);
      this.snackBar.open('Failed to initiate Google login. Please try again.', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar']
      });
    }
  }

  private handleOAuthCallback(code: string, state?: string): void {
    this.isLoading.set(true);

    this.authService.handleOAuthCallback(code, state).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        this.snackBar.open('Google login successful!', 'Close', { duration: 3000 });

        // Redirect to intended page or home
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigate([returnUrl]);
      },
      error: (error) => {
        this.isLoading.set(false);
        this.snackBar.open(error.message || 'Google login failed. Please try again.', 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });

        // Remove OAuth params from URL
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: {},
          replaceUrl: true
        });
      }
    });
  }

  onForgotPassword(): void {
    if (this.forgotPasswordForm.invalid) {
      this.markFormGroupTouched(this.forgotPasswordForm);
      return;
    }

    const email = this.forgotPasswordForm.value.email;

    this.authService.requestPasswordReset(email).subscribe({
      next: () => {
        this.snackBar.open('Password reset email sent! Check your inbox.', 'Close', {
          duration: 5000
        });
        this.showForgotPassword.set(false);
        this.forgotPasswordForm.reset();
      },
      error: (error) => {
        this.snackBar.open(error.message || 'Failed to send reset email. Please try again.', 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }

  togglePasswordVisibility(): void {
    this.hidePassword.set(!this.hidePassword());
  }

  toggleForgotPassword(): void {
    this.showForgotPassword.set(!this.showForgotPassword());
    if (this.showForgotPassword()) {
      // Pre-fill email if available
      const email = this.loginForm.get('email')?.value;
      if (email) {
        this.forgotPasswordForm.patchValue({ email });
      }
    }
  }

  goToRegister(): void {
    this.router.navigate(['/account/register']);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  // Getter methods for template
  get emailControl() { return this.loginForm.get('email'); }
  get passwordControl() { return this.loginForm.get('password'); }
  get forgotEmailControl() { return this.forgotPasswordForm.get('email'); }
}
