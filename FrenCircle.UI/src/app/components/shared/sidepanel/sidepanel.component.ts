/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit } from '@angular/core';
import { DndModule } from 'ngx-drag-drop';

// import { Offcanvas } from 'bootstrap';

@Component({
    selector: 'app-sidepanel',
    imports: [DndModule],
    templateUrl: './sidepanel.component.html',
    styleUrl: './sidepanel.component.css',
})
export class SidepanelComponent implements OnInit {
    ngOnInit(): void {
        // setTimeout(() => {
        //     const myOffcanvas = new Offcanvas(
        //         document.getElementById('customizer') as HTMLElement,
        //     );
        //     myOffcanvas.show();
        // }, 4000);
    }

    onDrag(index: number) {
        localStorage.setItem('draggedIndex', index.toString());
    }

    onDrop(event: any, targetIndex: number) {
        const sourceIndex = parseInt(
            localStorage.getItem('draggedIndex') || '0',
        );
        if (sourceIndex !== targetIndex) {
            const container = document.querySelector('.card-container');
            const cards = container?.children;
            if (cards) {
                container?.insertBefore(cards[sourceIndex], cards[targetIndex]);
            }
        }
    }
}
