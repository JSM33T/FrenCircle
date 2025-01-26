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

    // // Generic GET method that returns just the data from the response
    // get<T>(endpoint: string): Observable<APIResponse<T>> {
    //     const url = `${this.baseUrl}/${endpoint}`;
    //     return this.http.get<APIResponse<T>>(url).pipe(
    //         catchError(this.handleError), // Handle any errors
    //         map((response) => {
    //             // You can still check for the status, but return the full response
    //             if (response.status !== 200) {
    //                 throw new Error(response.message || 'Error fetching data');
    //             }
    //             return response; // Return the full response (status, message, and data)
    //         }),
    //     );
    // }

    // // Generic POST method
    // post<T>(endpoint: string, body: any): Observable<APIResponse<T>> {
    //     const url = `${this.baseUrl}/${endpoint}`;
    //     return this.http.post<APIResponse<T>>(url, body).pipe(
    //         catchError(this.handleError),
    //         map((response) => {
    //             if (response.status !== 200) {
    //                 throw new Error(response.message || 'Error posting data');
    //             }
    //             return response;
    //         }),
    //     );
    // }

    // // Simple error handler to catch errors and log them
    // private handleError(error: any): Observable<never> {
    //     console.error('API Error:', error);
    //     throw new Error('Something went wrong with the API request.');
    // }
    // Generic GET method that returns the full response (data and error)

    get<T>(endpoint: string): Observable<APIResponse<T>> {
        const url = `${this.baseUrl}/${endpoint}`;
        return this.http.get<APIResponse<T>>(url);
    }

    // Generic POST method
    post<T>(endpoint: string, body: any): Observable<APIResponse<T>> {
        const url = `${this.baseUrl}/${endpoint}`;
        return this.http.post<APIResponse<T>>(url, body);
    }
}
