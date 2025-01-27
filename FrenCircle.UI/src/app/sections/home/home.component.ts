import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MetaTagsService } from '../../services/Meta/meta-tags.service';
import { OffCanvasService } from '../../services/DOMServices/off-canvas.service';

@Component({
    selector: 'app-home',
    imports: [RouterLink],
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
})
export class HomeComponent {
    constructor(
        private metaService: MetaTagsService,
        private offCanvasService: OffCanvasService,
    ) {}

    ngOnInit(): void {
        console.log('home reached');
    }

    setMetaTags() {
        this.metaService.setMetaTags(
            "Jassi's web space",
            'Code, Music, Blogs and much more.',
        );
    }

    something() {
        this.offCanvasService.showOffcanvas('customizer');
    }
}
