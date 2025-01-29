/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component } from '@angular/core';
import { BreadcrumbsComponent } from '../../../components/ui/breadcrumbs/breadcrumbs.component';
import { NgIf } from '@angular/common';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    ReactiveFormsModule,
} from '@angular/forms';
import { ApiHandlerService } from '../../../services/Api/api-handler.service';
import { ModalService } from '../../../services/DOMServices/modal.service';

@Component({
    selector: 'app-login',
    imports: [ReactiveFormsModule, NgIf, BreadcrumbsComponent],
    templateUrl: './login.component.html',
    styleUrl: './login.component.css',
})
export class LoginComponent {
    loginForm!: FormGroup;
    isLoading = false;
    data: any;

    constructor(
        private apiService: ApiHandlerService,
        private fb: FormBuilder,
        private mdlService: ModalService,
    ) {
        this.loginForm = this.fb.group({
            username: new FormControl(''),
            password: new FormControl(''),
        });
    }

    onSubmit(): void {
        this.isLoading = true;

        this.apiService
            .post<any>('api/account/login', this.loginForm.value)
            .subscribe({
                next: (response) => {
                    if (response.status == 200) {
                        console.log(response.data);
                        localStorage.setItem('toiken', response.data.token);
                    }
                    this.mdlService.apiToaster(response);
                    this.loginForm.reset();
                },
                error: (error) => {
                    console.log(error);
                    this.mdlService.apiToaster(error.error);
                },
            });

        this.isLoading = false;
    }
}
