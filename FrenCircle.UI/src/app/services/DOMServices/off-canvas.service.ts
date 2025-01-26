import { Injectable } from '@angular/core';
import { Offcanvas } from 'bootstrap';

@Injectable({
    providedIn: 'root',
})
export class OffCanvasService {
    showOffcanvas(offcanvasElementId: string): void {
        const offcanvasElement = document.getElementById(offcanvasElementId);
        if (offcanvasElement) {
            const myOffcanvas = new Offcanvas(offcanvasElement);
            myOffcanvas.show();
        }
    }

    closeOffcanvas(offcanvasElementId: string): void {
        const offcanvasElement = document.getElementById(offcanvasElementId);
        if (offcanvasElement) {
            const myOffcanvas = Offcanvas.getInstance(offcanvasElement);
            if (myOffcanvas) {
                myOffcanvas.hide();
            }
        }
    }

    closeAllOffcanvases(): void {
        const allOffcanvasElements = document.querySelectorAll('.offcanvas');
        allOffcanvasElements.forEach((element) => {
            const offcanvasInstance = Offcanvas.getInstance(element);
            if (offcanvasInstance) {
                offcanvasInstance.hide();
            }
        });
    }
}
