/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { APIResponse } from '../../interfaces/APIResponse';

@Injectable({
    providedIn: 'root',
})
export class ApiHandlerService {
    private baseUrl: string = environment.urls.apiUrl; // Update this to your base API URL

    constructor(private http: HttpClient) {}

    get<T>(endpoint: string): Observable<APIResponse<T>> {
        const url = `${this.baseUrl}/${endpoint}`;
        return this.http.get<APIResponse<T>>(url, { withCredentials: true });
    }

    // Generic POST method
    post<T>(endpoint: string, body: any): Observable<APIResponse<T>> {
        const url = `${this.baseUrl}/${endpoint}`;
        return this.http.post<APIResponse<T>>(url, body, {
            withCredentials: true,
        });
    }

    refreshToken(): Observable<APIResponse<{ token: string }>> {
        return this.http.post<APIResponse<{ token: string }>>(
            `${this.baseUrl}/api/account/refresh`,
            {},
            { withCredentials: true },
        );
    }
}
