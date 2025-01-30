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
    imports: [ReactiveFormsModule, NgIf, BreadcrumbsComponent],
    templateUrl: './verify.component.html',
    styleUrl: './verify.component.css',
})
export class VerifyComponent {
    verifyForm!: FormGroup;
    isLoading = false;
    data: any;

    constructor(
        private apiService: ApiHandlerService,
        private fb: FormBuilder,
        private mdlService: ModalService,
    ) {
        this.verifyForm = this.fb.group({
            username: new FormControl(''),
            password: new FormControl(''),
        });
    }

    onSubmit(): void {
        this.isLoading = true;

        this.apiService
            .post<any>('api/account/login', this.verifyForm.value)
            .subscribe({
                next: (response) => {
                    if (response.status == 200) {
                        console.log(response.data);
                        localStorage.setItem('toiken', response.data.token);
                    }
                    this.mdlService.apiToaster(response);
                    this.verifyForm.reset();
                },
                error: (error) => {
                    console.log(error);
                    this.mdlService.apiToaster(error.error);
                },
            });

        this.isLoading = false;
    }
}
