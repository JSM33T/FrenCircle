import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NavbarService {
  private triggerUpdate = new Subject<void>();
  triggerUpdate$ = this.triggerUpdate.asObservable();

  triggerNavbarUpdate() {
    this.triggerUpdate.next();
  }
}
