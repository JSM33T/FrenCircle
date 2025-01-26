import { Component, OnInit } from '@angular/core';
import { AudiostreamComponent } from '../audiostream/audiostream.component';

// import { Offcanvas } from 'bootstrap';

@Component({
    selector: 'app-sidepanel',
    imports: [AudiostreamComponent],
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
