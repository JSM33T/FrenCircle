import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../services/HttpServices/http.service';
import { Observable } from 'rxjs';
import { APIResponse } from '../../models/IAPIResponse';
import { ResponseHandlerService } from '../../services/HttpServices/response-handler.service';
import { IGetPost } from '../../interfaces/IPosts';
import { RouterLink } from '@angular/router';
import { NgFor } from '@angular/common';

@Component({
	selector: 'app-wall',
	standalone: true,
	imports: [RouterLink, NgFor],
	templateUrl: './wall.component.html',
	styleUrl: './wall.component.css',
})
export class WallComponent implements OnInit {
	posts: IGetPost[] = [];

	constructor(private httpService: HttpService, private responseHandler: ResponseHandlerService) {}

	ngOnInit(): void {
		this.loadPosts();
	}

	loadPosts() {
		const response$: Observable<APIResponse<IGetPost[]>> = this.httpService.get('test');

		this.responseHandler.handleResponse(response$, false).subscribe({
			next: (response) => {
				console.log(response);
				this.posts = response.data;
			},
			error: (error) => {
				console.log(error.error);
			},
		});
	}
}
