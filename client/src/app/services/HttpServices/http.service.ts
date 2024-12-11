import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
	providedIn: 'root',
})
export class HttpService {
	private apiUrl = environment.urls.apiUrl;

	constructor(private http: HttpClient) {}

	fetchFile(url: string): Observable<any[]> {
		return this.http.get<any[]>(this.apiUrl + url);
	}

	// Function to get data from the API
	get<T>(endpoint: string): Observable<T> {
		return this.http.get<T>(`${this.apiUrl}/${endpoint}`);
	}

	// Function to post data to the API
	post<T>(endpoint: string, data: any, options?: { headers?: HttpHeaders }): Observable<T> {
		return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data, options);
	}

	//Function to read and map json file
	readJsonFile<T>(filePath: string): Observable<T> {
		return this.http.get<T>(filePath).pipe(
			map((response) => {
				// You can add any additional processing here if needed
				return response as T;
			}),
		);
	}
}
