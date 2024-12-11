import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetterService {

  constructor(private http: HttpClient) {}

  getJson(path : string): Observable<any[]> {
    return this.http.get<any[]>(path);
  }
}
