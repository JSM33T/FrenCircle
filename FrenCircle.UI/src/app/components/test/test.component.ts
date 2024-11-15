import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
	selector: 'app-test',
	standalone: true,
	imports: [],
	templateUrl: './test.component.html',
	styleUrl: './test.component.css'
})
export class TestComponent implements OnInit {
	slug: string = "";

	constructor(private route: ActivatedRoute) { }

	ngOnInit() {
		this.route.params.subscribe(params => {
			this.slug = params['slug'];
		});
	}
}
