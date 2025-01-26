/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component } from '@angular/core';
import { about, title } from './text';
import { MetaTagsService } from '../../services/Meta/meta-tags.service';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-about',
    imports: [RouterLink],
    templateUrl: './about.component.html',
    styleUrl: './about.component.css'
})
export class AboutComponent {

    constructor(private metaService: MetaTagsService) { }

    texts: any = {
        title: title,
        about: about,
    };

    ngOnInit(): void {

    }


    setMetaTags() {
        this.metaService.setMetaTags(
            'About | Jassi\'s web space'
        );
    }
}
