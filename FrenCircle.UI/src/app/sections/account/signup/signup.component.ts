/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component } from '@angular/core';
import { ApiHandlerService } from '../../../services/Api/api-handler.service';
import { ModalService } from '../../../services/DOMServices/modal.service';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    ReactiveFormsModule,
} from '@angular/forms';
import { BreadcrumbsComponent } from '../../../components/ui/breadcrumbs/breadcrumbs.component';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-signup',
    imports: [BreadcrumbsComponent, ReactiveFormsModule, NgIf],
    templateUrl: './signup.component.html',
    styleUrl: './signup.component.css',
})
export class SignupComponent {
    signUpForm!: FormGroup;
    isLoading = false;
    data: any;

    constructor(
        private apiService: ApiHandlerService,
        private fb: FormBuilder,
        private mdlService: ModalService,
    ) {
        this.signUpForm = this.fb.group({
            email: new FormControl(''),
            firstname: new FormControl(''),
            lastname: new FormControl(''),
            username: new FormControl(''),
            password: new FormControl(''),
            confirmpassword: new FormControl(''),
        });
    }

    onSubmit(): void {
        this.isLoading = true;

        if (
            this.signUpForm.get('password')?.value !=
            this.signUpForm.get('confirmpassword')?.value
        ) {
            this.mdlService.toast("Passwords don't match");
            this.isLoading = false;
            return;
        }

        console.log(this.signUpForm.value);
        this.apiService
            .post<any>('api/account/create', this.signUpForm.value)
            .subscribe({
                next: (response) => {
                    if (response.status == 200) {
                        console.log(response.data);
                        localStorage.setItem('token', response.data.token);
                    }
                    this.isLoading = false;
                    this.mdlService.apiToaster(response);
                    this.signUpForm.reset();
                },
                error: (error) => {
                    this.isLoading = false;
                    this.mdlService.apiToaster(error.error);
                },
            });
    }
}
