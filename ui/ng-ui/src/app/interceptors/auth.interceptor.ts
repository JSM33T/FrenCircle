import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { environment } from '../../environments/environment';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.accessToken();

  // Check if this is an API request to our backend
  const baseUrl = environment.apiUrl || 'https://localhost:7185/api';
  const isApiRequest = req.url.startsWith(baseUrl);

  let modifiedReq = req;

  // Always include credentials for API requests to handle cookies
  if (isApiRequest) {
    console.log('Setting withCredentials for API request:', req.url);
    modifiedReq = req.clone({
      setHeaders: token ? { 'Authorization': `Bearer ${token}` } : {},
      withCredentials: true // This ensures cookies are sent with every API request
    });
  } else if (token) {
    // For non-API requests, just add the token
    modifiedReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }

  return next(modifiedReq);
};
