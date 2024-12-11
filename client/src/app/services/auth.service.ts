import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.urls.apiUrl + '/api/account';

  constructor(private http: HttpClient) { }

  googleLogin(idToken: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/google-login`, { idToken });
  }
}