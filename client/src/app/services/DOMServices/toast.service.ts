import { Injectable } from '@angular/core';
import { APIResponse } from '../../models/IAPIResponse';
import * as bootstrap from 'bootstrap';

@Injectable({
	providedIn: 'root',
})
export class ToastService {
	showToast(response: APIResponse<any>): void {
		const statusTitles: { [key: number]: string } = {
			200: 'OK',
			201: 'OK',
			400: 'BAD_REQUEST',
			401: 'UNAUTHORIZED',
			403: 'FORBIDDEN',
			404: 'NOT_FOUND',
			410: 'EXPIRED',
			500: 'SERVER_ERROR',
			429: 'SPAM_DETECTED',
		};

		const titleKey = statusTitles[response.status] || 'Notif';
		const hints = response.hints || [];
		const message = response.message || 'An error occurred';

		this.showModal(titleKey, message, hints, response.status === 400);
	}
    acToaster(title: string, message: string, durationInSeconds: number = 5) {
        let currentToast: any = null;
        // Destroy the existing toast if there is one
        if (currentToast) {
            currentToast.hide();
        }
    
        // Create the toast container
        const toastContainer = document.createElement('div');
        toastContainer.classList.add('toast');
        toastContainer.setAttribute('role', 'alert');
        toastContainer.setAttribute('aria-live', 'assertive');
        toastContainer.setAttribute('aria-atomic', 'true');
    
        // Set the inner HTML of the toast container using template string
        toastContainer.innerHTML = `
            <div class="toast-header">
                <strong class="me-auto">${title}</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        `;
    
        // Create or get the toast container wrapper
        let toastContainerWrapper = document.getElementById('toast-container-wrapper');
        if (!toastContainerWrapper) {
            toastContainerWrapper = document.createElement('div');
            toastContainerWrapper.id = 'toast-container-wrapper';
            toastContainerWrapper.style.top = '20px';
            toastContainerWrapper.style.display = 'flex';
            toastContainerWrapper.style.justifyContent = 'center';
            document.body.appendChild(toastContainerWrapper);
        }
    
        // Clear the wrapper of any existing toasts
        toastContainerWrapper.innerHTML = '';
    
        // Append the new toast to the container
        toastContainerWrapper.appendChild(toastContainer);
    
        // Initialize the toast using Bootstrap's JavaScript API
        const toastInstance = new bootstrap.Toast(toastContainer, {
            autohide: true,
            delay: durationInSeconds * 1000, // Convert seconds to milliseconds
        });
    
        // Show the toast
        toastInstance.show();
    
        // Set the current toast
        currentToast = toastInstance;
    
        // Remove the toast from the DOM after it's hidden
        toastContainer.addEventListener('hidden.bs.toast', function () {
            if (toastContainerWrapper!.contains(toastContainer)) {
                toastContainerWrapper!.removeChild(toastContainer);
            }
            // Remove the wrapper if it's empty
            if (toastContainerWrapper!.children.length === 0) {
                document.body.removeChild(toastContainerWrapper!);
            }
            // Clear the current toast reference
            if (currentToast === toastInstance) {
                currentToast = null;
            }
        });
    }
	private showModal(title: string, message: string, hints: string[], someBoolean: boolean): void {
		const modalContainer = document.createElement('div');
		modalContainer.classList.add('modal', 'fade');
		modalContainer.setAttribute('tabindex', '-1');
		modalContainer.setAttribute('role', 'dialog');
		modalContainer.setAttribute('aria-hidden', 'true');
		modalContainer.setAttribute('data-bs-backdrop', 'static');

		let errorListHtml = '';
		if (hints && hints.length > 0) {
			errorListHtml = '<code class="text-primary">';
			hints.forEach((hint) => {
				errorListHtml += `<li>${hint}</li>`;
			});
			errorListHtml += '</code>';
		}

		if (someBoolean) {
			// Handle the boolean logic here if needed
		}

		modalContainer.innerHTML = `
      <div class="modal-dialog" role="document" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">${title}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <p>${message}</p>
            <span>${errorListHtml}</span>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
             OK
            </button>
          </div>
        </div>
      </div>
    `;

		document.body.appendChild(modalContainer);

		const modal = new bootstrap.Modal(modalContainer);
		modal.show();

		modalContainer.addEventListener('hidden.bs.modal', function () {
			document.body.removeChild(modalContainer);
		});
	}
}
