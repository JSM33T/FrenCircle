import { Injectable } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';

@Injectable({
	providedIn: 'root'
})
export class MetaTagsService {

	private defaultTitle = 'Jassi\'s Webspace';
	private defaultDescription = 'Music , Code, Blogs and beyond.';

	constructor(private meta: Meta, private title: Title) { }

	setMetaTags(title?: string, description?: string): void {
		// Set the title (use the provided one or default)
		this.title.setTitle(title || this.defaultTitle);

		// Set the description (use the provided one or default)
		this.meta.updateTag({ name: 'description', content: description || this.defaultDescription });

		// You can add other default meta tags here (e.g., for Open Graph)
		this.meta.updateTag({ name: 'robots', content: 'index, follow' });
	}
}
