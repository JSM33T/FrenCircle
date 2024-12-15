import { Component, NgZone, OnInit } from '@angular/core';
import { ThemeEngineService } from '../../../services/themeEngine/themeengine.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter, Observable } from 'rxjs';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { APIResponse } from '../../../models/IAPIResponse';
import { HttpService } from '../../../services/HttpServices/http.service';
import { ResponseHandlerService } from '../../../services/HttpServices/response-handler.service';

@Component({
	selector: 'app-sidepanel',
	standalone: true,
	imports: [ReactiveFormsModule, NgIf],
	templateUrl: './sidepanel.component.html',
	styleUrl: './sidepanel.component.css',
})
export class SidepanelComponent implements OnInit {
	reportForm: FormGroup;
	isLoading: boolean = false;

	constructor(
		private router: Router,
		private fb: FormBuilder,
		private httpService: HttpService,
		private responseHandler: ResponseHandlerService,
		private dynamicStyleService: ThemeEngineService,
		private zone: NgZone
	) {
		this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
			window.scrollTo({
				top: 0,
				behavior: 'smooth',
			});
		});

		this.reportForm = this.fb.group({
			report: ['', [Validators.required]],
		});
	}

	ngOnInit(): void {}

	applyTheme() {
		const css = `
		:root{--ar-primary:#586889;--ar-primary-rgb:88,104,137;--ar-border-radius:0rem;--ar-link-color:#586889;--ar-link-hover-color:#3f4f70;}.btn-primary{--ar-btn-bg:#586889;--ar-btn-border-color:#586889;--ar-btn-hover-bg:#3f4f70;--ar-btn-hover-border-color:#3f4f70;--ar-btn-active-bg:#3f4f70;--ar-btn-active-border-color:#3f4f70;--ar-btn-disabled-bg:#586889;--ar-btn-disabled-border-color:#586889;}.btn-outline-primary{--ar-btn-color:#586889;--ar-btn-border-color:#586889;--ar-btn-hover-bg:#586889;--ar-btn-hover-border-color:#586889;--ar-btn-active-bg:#586889;--ar-btn-active-border-color:#586889;--ar-btn-disabled-color:#586889;--ar-btn-disabled-border-color:#586889;}.accordion-button:not(.collapsed)::after{--ar-accordion-btn-active-icon:url("data:image/svg+xml,%3csvg%20xmlns='http://www.w3.org/2000/svg'%20viewBox='0%200%2024%2024'%20fill='%23586889'%3e%3cpath%20d='M.5%206.3c.6-.6%201.6-.6%202.3%200l9.3%209.3%209.3-9.3c.6-.6%201.6-.6%202.3%200%20.6.6.6%201.6%200%202.3L13.3%2018.8c-.6.6-1.6.6-2.3%200L.8%208.5c-.9-.8-.9-1.6-.3-2.2z'/%3e%3c/svg%3e");}
	   `;

		const font = `https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap`;
	}

	setFOnt(size: number) {
		this.dynamicStyleService.applyFont(size);
	}

	submitReport() {
		if (this.reportForm.valid) {
			const report = {
				report: this.reportForm.get('report')?.value,
				source: 'reportpanel',
			};

			const response$: Observable<APIResponse<any>> = this.httpService.post('api/contact/report', report);
			console.log(report);
			this.responseHandler.handleResponse(response$, true).subscribe({
				next: (response) => {
					this.isLoading = false;
					if (response.status == 200) {
						console.log(response.data);
					}
				},
				error: (error) => {
					console.log(error.error);
					this.isLoading = false;
				},
			});
		}
	}
}
