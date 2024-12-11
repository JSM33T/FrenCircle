import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { NgIf } from '@angular/common';
import { FormBuilder, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';

import { RouterModule } from '@angular/router';
import { HttpService } from '../../services/HttpServices/http.service';
import { ResponseHandlerService } from '../../services/HttpServices/response-handler.service';
import { APIResponse } from '../../models/IAPIResponse';

@Component({
	selector: 'app-contact',
	standalone: true,
	imports: [ReactiveFormsModule, NgIf, RouterModule],
	templateUrl: './contact.component.html',
})
export class ContactComponent implements OnInit, OnDestroy {
	messageForm!: FormGroup;
	isLoading = false;

	constructor(private httpService: HttpService, private fb: FormBuilder, private responseHandler: ResponseHandlerService) {
		this.messageForm = this.fb.group({
			name: new FormControl(''),
			email: new FormControl(''),
			topic: new FormControl('general'),
			origin: new FormControl(''),
			content: new FormControl(''),
		});
	}

	ngOnInit(): void {}

	ngOnDestroy(): void {}

	onSubmit(): void {
		this.isLoading = true;
		const response$: Observable<APIResponse<any>> = this.httpService.post('api/messages/send', this.messageForm.value);

		this.responseHandler.handleResponse(response$, true).subscribe({
			next: (response) => {
				//console.log(response);
				this.isLoading = false;
			},
			error: (error) => {
				console.log(error.error);
				this.isLoading = false;
			},
		});
	}
}
