/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component } from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    ReactiveFormsModule,
} from '@angular/forms';
import { ApiHandlerService } from '../../../services/Api/api-handler.service';
import { ModalService } from '../../../services/DOMServices/modal.service';
import { NgIf } from '@angular/common';
import { BreadcrumbsComponent } from '../../../components/ui/breadcrumbs/breadcrumbs.component';

@Component({
    selector: 'app-contact',
    imports: [ReactiveFormsModule, NgIf, BreadcrumbsComponent],
    templateUrl: './contact.component.html',
    styleUrl: './contact.component.css',
})
export class ContactComponent {
    messageForm!: FormGroup;
    isLoading = false;
    data: any;

    constructor(
        private apiService: ApiHandlerService,
        private fb: FormBuilder,
        private mdlService: ModalService,
    ) {
        this.messageForm = this.fb.group({
            name: new FormControl(''),
            email: new FormControl(''),
            origin: new FormControl(''),
            text: new FormControl(''),
        });
    }

    onSubmit(): void {
        this.isLoading = true;

        this.apiService
            .post<any>('api/message/send', this.messageForm.value)
            .subscribe({
                next: (response) => {
                    this.mdlService.apiToaster(response);
                    this.messageForm.reset();
                },
                error: (error) => {
                    console.log(error);
                    this.mdlService.apiToaster(error.error);
                },
            });

        this.isLoading = false;
    }
}
