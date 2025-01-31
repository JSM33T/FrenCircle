/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component } from '@angular/core';
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

@Component({
    selector: 'app-verify',
    standalone: true,
    imports: [ReactiveFormsModule, NgIf, BreadcrumbsComponent],
    templateUrl: './verify.component.html',
    styleUrl: './verify.component.css',
})
export class VerifyComponent {
    verifyForm!: FormGroup;
    isLoading = false;
    isVerificationStep = false;
    buttonText = 'Send OTP';
    data: any;

    constructor(
        private apiService: ApiHandlerService,
        private fb: FormBuilder,
        private mdlService: ModalService,
    ) {
        this.verifyForm = this.fb.group({
            email: new FormControl(''),
            otp: new FormControl(''),
        });
        this.isVerificationStep = false;
    }

    stepOneRequest() {}
    stepTwoVerify() {}
    reset() {}

    onOtpChange(): void {
        if (
            this.verifyForm.get('otp')?.value !== 0 ||
            this.verifyForm.get('otp')?.value.trim() !== ''
        ) {
            this.buttonText = 'Verify';
        } else {
            this.buttonText = 'Send OTP';
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
                            this.stepTwoVerify();
                            this.buttonText = 'Verify';
                        }
                    },
                    error: (error) => {
                        this.isLoading = false;
                        this.mdlService.apiToaster(error.error);
                    },
                });
        } else if (email !== '' && otp !== 0) {
            this.isVerificationStep = true;
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
                        this.reset();
                        this.verifyForm.reset();
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
