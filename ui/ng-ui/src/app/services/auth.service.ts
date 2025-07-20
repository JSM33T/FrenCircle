import { Injectable, signal, computed, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, throwError, of } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { environment } from '../../environments/environment';

// Interfaces matching your backend DTOs
export interface User {
  id: string;
  email: string;
  username?: string;
  firstName?: string;
  lastName?: string;
  avatar?: string;
  emailVerified: boolean;
  twoFactorEnabled: boolean;
  timezone: string;
  locale: string;
  roles: string[];
  permissions: string[];
  createdAt: string;
  lastLoginAt?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
  deviceFingerprint?: string;
  rememberMe?: boolean;
  twoFactorCode?: string;
}

export interface RegisterRequest {
  email: string;
  username?: string;
  password: string;
  firstName?: string;
  lastName?: string;
  timezone?: string;
  locale?: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

export interface LoginResponse {
  user: User;
  accessToken: string;
  tokenType: string;
  expiresInMinutes: number;
  loginMethod?: string;
}

export interface OAuthUrlResponse {
  authUrl: string;
  state: string;
  provider: string;
}

export interface ApiResponse<T = any> {
  status: number;
  message: string;
  data: T;
  hints?: string[];
  responseTimeMs?: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http: HttpClient;
  private readonly router: Router;
  private readonly platformId: Object;

  private readonly baseUrl = environment.apiUrl || 'https://localhost:7001/api';

  // Reactive state using signals
  private readonly _currentUser = signal<User | null>(null);
  private readonly _isAuthenticated = signal<boolean>(false);
  private readonly _accessToken = signal<string | null>(null);

  // Public readonly signals
  readonly currentUser = this._currentUser.asReadonly();
  readonly isAuthenticated = this._isAuthenticated.asReadonly();
  readonly accessToken = this._accessToken.asReadonly();  // Computed properties
  readonly isAdmin = computed(() =>
    this._currentUser()?.roles?.includes('Admin') ?? false
  );

  readonly userInitials = computed(() => {
    const user = this._currentUser();
    if (!user) return '';

    const first = user.firstName?.charAt(0) || '';
    const last = user.lastName?.charAt(0) || '';

    return (first + last).toUpperCase() || user.email.charAt(0).toUpperCase();
  });

  constructor() {
    this.http = inject(HttpClient);
    this.router = inject(Router);
    this.platformId = inject(PLATFORM_ID);

    // Don't initialize immediately in constructor - let APP_INITIALIZER handle it
    // this.initializeAuth();

    // Start token refresh timer after a longer delay to allow proper initialization
    setTimeout(() => {
      this.startTokenRefreshTimer();
    }, 5000);
  }

  private initializeAuth(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return; // Skip initialization on server
    }

    const token = this.getFromStorage('accessToken');
    const storedUser = this.getFromStorage('user');

    if (token && storedUser) {
      try {
        const user = JSON.parse(storedUser);
        this._accessToken.set(token);
        this._currentUser.set(user);
        this._isAuthenticated.set(true);

        // Immediately try to refresh token to ensure it's still valid
        this.refreshToken().subscribe({
          next: (response) => {
            // Token refreshed successfully, user state is maintained
            console.log('Auth state restored and token refreshed');
          },
          error: (error) => {
            console.warn('Token refresh failed on initialization, logging out:', error);
            this.logout();
          }
        });
      } catch (error) {
        console.error('Failed to parse stored user data:', error);
        this.logout();
      }
    } else if (token) {
      // We have a token but no user data, try to refresh and get user info
      this._accessToken.set(token);
      this.refreshToken().subscribe({
        error: () => {
          console.warn('Token invalid on initialization, logging out');
          this.logout();
        }
      });
    }
  }

  private startTokenRefreshTimer(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return; // Skip timer on server
    }

    // Refresh token every 25 minutes (token expires in 30)
    const refreshInterval = setInterval(() => {
      if (this._isAuthenticated() && this._accessToken()) {
        console.log('Auto-refreshing token...');
        this.refreshToken().subscribe({
          next: () => {
            console.log('Token auto-refreshed successfully');
          },
          error: (error) => {
            console.error('Auto token refresh failed:', error);
            clearInterval(refreshInterval);
            this.logout();
          }
        });
      } else {
        console.log('Skipping token refresh - user not authenticated');
        clearInterval(refreshInterval);
      }
    }, 25 * 60 * 1000);
  }

  // Helper methods for safe localStorage access
  private getFromStorage(key: string): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    return localStorage.getItem(key);
  }

  private setInStorage(key: string, value: string): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }
    localStorage.setItem(key, value);
  }

  private removeFromStorage(key: string): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }
    localStorage.removeItem(key);
  }

  // Generate device fingerprint for session tracking
  private generateDeviceFingerprint(): string {
    if (!isPlatformBrowser(this.platformId)) {
      // Fallback for SSR
      return 'ssr-' + Math.random().toString(36).substring(2, 15);
    }

    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');
    ctx!.textBaseline = 'top';
    ctx!.font = '14px Arial';
    ctx!.fillText('Device fingerprint', 2, 2);

    const fingerprint = [
      navigator.userAgent,
      navigator.language,
      screen.width + 'x' + screen.height,
      new Date().getTimezoneOffset(),
      canvas.toDataURL()
    ].join('|');

    return btoa(fingerprint).substring(0, 32);
  }

  // Regular email/password login
  login(credentials: LoginRequest): Observable<LoginResponse> {
    const deviceFingerprint = this.generateDeviceFingerprint();

    const loginData = {
      ...credentials,
      deviceFingerprint
    };

    console.log('Login request with withCredentials');
    return this.http.post<ApiResponse<LoginResponse>>(`${this.baseUrl}/auth/login`, loginData, {
      withCredentials: true
    }).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'Login failed');
        }
      }),
      tap(loginResponse => {
        this.setAuthData(loginResponse);
      }),
      catchError(this.handleError)
    );
  }

  // Register new user
  register(userData: RegisterRequest): Observable<any> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/auth/register`, userData).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'Registration failed');
        }
      }),
      catchError(this.handleError)
    );
  }

  // Start Google OAuth flow
  getGoogleAuthUrl(): Observable<OAuthUrlResponse> {
    const redirectUri = environment.oauth.google.redirectUri;
    const state = this.generateDeviceFingerprint();

    return this.http.get<ApiResponse<OAuthUrlResponse>>(
      `${this.baseUrl}/auth/oauth/google`,
      {
        params: { redirectUri, state }
      }
    ).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'OAuth URL generation failed');
        }
      }),
      catchError(this.handleError)
    );
  }

  // Handle OAuth callback
  handleOAuthCallback(code: string, state?: string): Observable<LoginResponse> {
    const redirectUri = environment.oauth.google.redirectUri;

    console.log('OAuth callback with withCredentials');
    return this.http.post<ApiResponse<LoginResponse>>(`${this.baseUrl}/auth/oauth/callback`, {
      provider: 'google',
      code,
      redirectUri,
      state
    }, {
      withCredentials: true
    }).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'OAuth callback failed');
        }
      }),
      tap(loginResponse => {
        this.setAuthData(loginResponse);
      }),
      catchError(this.handleError)
    );
  }

  // Refresh access token
  refreshToken(): Observable<any> {
    console.log('Calling refresh token with withCredentials');
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/auth/refresh`, {}, {
      withCredentials: true
    }).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'Token refresh failed');
        }
      }),
      tap((responseData: any) => {
        console.log('Refresh token successful, response:', responseData);
        if (responseData.accessToken) {
          this._accessToken.set(responseData.accessToken);
          this.setInStorage('accessToken', responseData.accessToken);
        }

        // If response includes user data, update it
        if (responseData.user) {
          this._currentUser.set(responseData.user);
          this._isAuthenticated.set(true);
          this.setInStorage('user', JSON.stringify(responseData.user));
        }
      }),
      catchError((error) => {
        console.error('Refresh token failed:', error);
        // Clear auth state when refresh fails
        this._currentUser.set(null);
        this._isAuthenticated.set(false);
        this._accessToken.set(null);
        this.removeFromStorage('accessToken');
        this.removeFromStorage('user');
        return this.handleError(error);
      })
    );
  }

  // Logout
  logout(): void {
    console.log('Logging out user...');

    // Clear local state
    this._currentUser.set(null);
    this._isAuthenticated.set(false);
    this._accessToken.set(null);

    // Clear localStorage
    this.removeFromStorage('accessToken');
    this.removeFromStorage('user');

    // Call backend logout (optional, since refresh token is httpOnly)
    this.http.post(`${this.baseUrl}/auth/logout`, {}).subscribe({
      next: () => console.log('Backend logout successful'),
      error: (error) => console.warn('Backend logout failed:', error)
    });

    // Redirect to login
    this.router.navigate(['/account']);
  }

  // Explicitly restore authentication state
  restoreAuthState(): Observable<boolean> {
    if (!isPlatformBrowser(this.platformId)) {
      return of(false);
    }

    const token = this.getFromStorage('accessToken');
    const storedUser = this.getFromStorage('user');

    if (!token) {
      return of(false);
    }

    if (storedUser) {
      try {
        const user = JSON.parse(storedUser);
        this._accessToken.set(token);
        this._currentUser.set(user);
        this._isAuthenticated.set(true);
      } catch (error) {
        console.error('Failed to parse stored user data:', error);
        this.logout();
        return of(false);
      }
    }

    // Verify token is still valid
    return this.refreshToken().pipe(
      map(() => {
        console.log('Auth state restored successfully');
        return true;
      }),
      catchError((error) => {
        console.warn('Failed to restore auth state:', error);
        this.logout();
        return of(false);
      })
    );
  }

  // Check if user should be authenticated (has valid session)
  checkAuthStatus(): Observable<boolean> {
    if (!isPlatformBrowser(this.platformId)) {
      return of(false);
    }

    const token = this.getFromStorage('accessToken');
    if (!token) {
      return of(false);
    }

    // Try to refresh token to verify authentication
    return this.refreshToken().pipe(
      map(() => true),
      catchError(() => {
        this.logout();
        return of(false);
      })
    );
  }

  // Get current user sessions
  // Get current user sessions
  getSessions(): Observable<any[]> {
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/auth/sessions`).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'Failed to get sessions');
        }
      }),
      catchError(this.handleError)
    );
  }

  // Revoke all other sessions
  revokeAllSessions(exceptSessionId?: string): Observable<any> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/auth/revoke-all-sessions`, {
      exceptSessionId
    }).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'Failed to revoke sessions');
        }
      }),
      catchError(this.handleError)
    );
  }

  // Request password reset
  requestPasswordReset(email: string): Observable<any> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/auth/reset-password/request`, {
      email
    }).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'Password reset request failed');
        }
      }),
      catchError(this.handleError)
    );
  }

  // Confirm password reset
  confirmPasswordReset(token: string, email: string, newPassword: string): Observable<any> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/auth/reset-password/confirm`, {
      token,
      email,
      newPassword
    }).pipe(
      map(response => {
        if (response.status >= 200 && response.status < 300) {
          return response.data;
        } else {
          throw new Error(response.message || 'Password reset confirmation failed');
        }
      }),
      catchError(this.handleError)
    );
  }

  // Request email verification
  requestEmailVerification(email: string): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.baseUrl}/auth/verify-email/request`, {
      email
    }).pipe(
      catchError(this.handleError)
    );
  }

  // Confirm email verification
  confirmEmailVerification(token: string, email: string): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.baseUrl}/auth/verify-email/confirm`, {
      token,
      email
    }).pipe(
      catchError(this.handleError)
    );
  }

  private setAuthData(response: LoginResponse): void {
    this._accessToken.set(response.accessToken);
    this._currentUser.set(response.user);
    this._isAuthenticated.set(true);

    // Store in localStorage for persistence
    this.setInStorage('accessToken', response.accessToken);
    this.setInStorage('user', JSON.stringify(response.user));

    console.log('Auth data set successfully:', {
      user: response.user.email,
      tokenLength: response.accessToken.length
    });
  }

  private handleError = (error: HttpErrorResponse): Observable<never> => {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.error?.errorMessage) {
        errorMessage = error.error.errorMessage;
      } else {
        errorMessage = `Server error: ${error.status} ${error.statusText}`;
      }
    }

    console.error('Auth service error:', error);
    return throwError(() => new Error(errorMessage));
  };
}
