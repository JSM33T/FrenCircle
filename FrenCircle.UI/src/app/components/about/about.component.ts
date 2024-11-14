import { Component } from '@angular/core';
import { Meta } from '@angular/platform-browser';

@Component({
	selector: 'app-about',
	standalone: true,
	imports: [],
	templateUrl: './about.component.html',
	styleUrl: './about.component.css'
})
export class AboutComponent {
	constructor(private meta: Meta) {
		this.meta.addTag({ name: 'description', content: 'A Micro blogging platform' });
		this.meta.addTag({ name: 'keywords', content: 'social,community,frencircle,posts,microblog,microstory' });
		this.meta.addTag({ property: 'og:title', content: 'About : FrenCircle' });
		this.meta.addTag({ property: 'og:description', content: 'A Micro blogging platform' });
		this.meta.addTag({ property: 'og:image', content: 'https://example.com/image.jpg' });
	}
}
