/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit } from '@angular/core';
// import { Offcanvas } from 'bootstrap';
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';
@Component({
    selector: 'app-sidepanel',
    imports: [CdkDrag, CdkDragHandle],
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
}
