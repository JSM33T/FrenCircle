/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable } from '@angular/core';
import { Modal } from 'bootstrap';

interface APIResponse<T> {
    status: number;
    message: string;
    data: T;
    hints?: string[];
}

@Injectable({
    providedIn: 'root',
})
export class ModalService {
    // Open a specific modal by ID
    openModal(modalId: string): void {
        const modalElement = document.getElementById(modalId);
        if (modalElement) {
            const modal = new Modal(modalElement);
            modal.show();
        } else {
            console.error(`Modal with ID ${modalId} not found.`);
        }
    }
    // Close a specific modal by ID
    closeModal(modalId: string): void {
        const modalElement = document.getElementById(modalId);
        if (modalElement) {
            const modal = Modal.getInstance(modalElement); // Get the existing modal instance
            if (modal) {
                modal.hide(); // Close the modal
            } else {
                console.error(`Modal instance for ID ${modalId} not found.`);
            }
        } else {
            console.error(`Modal with ID ${modalId} not found.`);
        }
    }

    // Close all modals
    closeAllModals(): void {
        const modals = document.querySelectorAll('.modal');
        modals.forEach((modalElement) => {
            const modal = Modal.getInstance(modalElement); // Get the existing modal instance
            if (modal) {
                modal.hide(); // Close the modal
            }
        });
    }

    // Toggle a specific modal by ID (open if closed, close if open)
    toggleModal(modalId: string): void {
        const modalElement = document.getElementById(modalId);
        if (modalElement) {
            const modal = Modal.getInstance(modalElement) as Modal; // Get the existing modal instance
            if (modalElement.classList.contains('show')) {
                modal.hide(); // Close if already open
            } else {
                modal.show(); // Open if already closed
            }
        } else {
            console.error(`Modal with ID ${modalId} not found.`);
        }
    }

    // Check if a modal is open
    isModalOpen(modalId: string): boolean {
        const modalElement = document.getElementById(modalId);
        if (modalElement) {
            return modalElement.classList.contains('show');
        }
        console.error(`Modal with ID ${modalId} not found.`);
        return false;
    }
    // Create and show a toaster-like notification (modal)
    toaster(
        toastId: string,
        title: string,
        body: string,
        hints: string[] = [],
    ): void {
        const toastContainer = document.createElement('div');
        toastContainer.classList.add('modal', 'fade');
        toastContainer.id = toastId;
        toastContainer.setAttribute('tabindex', '-1');
        toastContainer.setAttribute('aria-labelledby', `${toastId}Label`);
        toastContainer.setAttribute('aria-hidden', 'true');

        // Construct the modal content
        let hintsContent = '';
        if (hints.length > 0) {
            hintsContent = hints.map((hint) => `<p>${hint}</p>`).join('');
        }

        toastContainer.innerHTML = `
      <div class="modal-dialog show" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="${toastId}Label">${title}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <p>${body}</p>
             <code>${hintsContent}</code>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    `;
        // Add the newly created toast (modal) to the document
        document.body.appendChild(toastContainer);

        this.closeAllModals();
        // Initialize and show the modal
        const modal = new Modal(toastContainer);
        modal.show();

        // Remove the modal after it has been hidden
        toastContainer.addEventListener('hidden.bs.modal', () => {
            toastContainer.remove();
        });
    }

    apiToaster(response: any) {
        console.log('toaster message herer');

        this.toaster(
            'toast_2',
            'Notification',
            response.message,
            response.hints,
        );
    }

    toast(message: string) {
        this.toaster(
            'toast_n',
            'Toast',
            message,
            [],
        );
    }
}
