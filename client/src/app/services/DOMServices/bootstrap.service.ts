import { Injectable } from '@angular/core';
import * as bootstrap from 'bootstrap';

@Injectable({
  providedIn: 'root',
})
export class BootstrapService {
  closeAllModals(): void {
    // Close all visible modals
    const modals = document.querySelectorAll('.modal.show');
    modals.forEach((modal) => {
      const modalInstance = bootstrap.Modal.getInstance(modal);
      if (modalInstance) {
        modalInstance.hide();
      }
    });

    // Remove all modal backdrops
    const backdrops = document.querySelectorAll('.modal-backdrop');
    backdrops.forEach((backdrop) => {
      backdrop.remove();
    });

    // Ensure the body doesn't retain modal-specific styles
    document.body.classList.remove('modal-open');
    document.body.style.overflow = '';
    document.body.style.paddingRight = '';
  }
}
