/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit } from '@angular/core';
import { BreadcrumbsComponent } from '../../../components/ui/breadcrumbs/breadcrumbs.component';
import { NgIf } from '@angular/common';
import {
    ReactiveFormsModule,
    FormGroup,
    FormBuilder,
    FormControl,
} from '@angular/forms';
import { ApiHandlerService } from '../../../services/Api/api-handler.service';
import { ModalService } from '../../../services/DOMServices/modal.service';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
    selector: 'app-verify',
    standalone: true,
    imports: [ReactiveFormsModule, NgIf, BreadcrumbsComponent],
    templateUrl: './verify.component.html',
    styleUrl: './verify.component.css',
})
export class VerifyComponent implements OnInit {
    verifyForm!: FormGroup;
    isLoading = false;
    buttonText = 'Send OTP';
    data: any;
    paramUsername: string = '';
    constructor(
        private apiService: ApiHandlerService,
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private mdlService: ModalService,
    ) {
        this.verifyForm = this.fb.group({
            email: new FormControl(''),
            otp: new FormControl(''),
        });
    }
    ngOnInit(): void {
        this.route.queryParams.subscribe((params) => {
            if (params['username']) {
                this.verifyForm.get('email')?.setValue(params['username']);
                this.paramUsername = params['username'];
                this.stepTwo();
                this.buttonText = 'Verify';
                this.onSubmit();
            }
        });
    }
    stepOne() {
        if (this.paramUsername === '') {
            this.buttonText = 'Send OTP';
        }
    }
    stepTwo() {
        this.buttonText = 'Verify';
    }
    reset() {}
    onStateChange(): void {
        console.log('Email===' + this.verifyForm.get('email')?.value);
        console.log('OTP====' + this.verifyForm.get('otp')?.value);
        if (
            this.verifyForm.get('email')?.value.trim() !== '' &&
            this.verifyForm.get('otp')?.value !== null
        ) {
            this.stepTwo();
        } else {
            this.stepOne();
        }
    }
    onSubmit(): void {
        this.isLoading = true;
        const email = this.verifyForm.get('email')?.value || '';
        const otp = this.verifyForm.get('otp')?.value || 0;
        console.log(this.verifyForm.value);
        if (email !== '' && otp === 0) {
            this.apiService
                .post<any>('api/account/generate-otp', this.verifyForm.value)
                .subscribe({
                    next: (response) => {
                        console.log(response, 'GENERATE OTP RESPONSE');
                        this.isLoading = false;
                        this.mdlService.apiToaster(response);
                        if (response.status == 200) {
                            this.isLoading = false;
                            this.stepTwo();
                        }
                    },
                    error: (error) => {
                        this.isLoading = false;
                        this.mdlService.apiToaster(error.error);
                    },
                });
        } else if (email !== '' && otp !== 0) {
            this.apiService
                .post<any>('api/account/verify', this.verifyForm.value)
                .subscribe({
                    next: (response) => {
                        console.log(response, 'VERIFICATION RESPONSE');
                        this.mdlService.apiToaster(response);
                        this.isLoading = false;
                        if (response.status == 200) {
                            console.log(response.data);
                            localStorage.setItem('token', response.data.token);
                        }
                        //this.router.navigate(['/']);
                        window.location.href = '/';
                    },
                    error: (error) => {
                        this.isLoading = false;
                        this.mdlService.apiToaster(error.error);
                    },
                });
        } else {
            this.mdlService.toast('email field is required');
        }
    }
}
