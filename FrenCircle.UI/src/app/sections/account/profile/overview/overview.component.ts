/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiHandlerService } from '../../../../services/Api/api-handler.service';

export interface Profile {
    firstName: string;
    lastName?: string;
    bio?: number;
    userName: string;
    email: string;
}

@Component({
    selector: 'app-overview',
    imports: [RouterLink],
    templateUrl: './overview.component.html',
    styleUrl: './overview.component.css',
})
export class OverviewComponent implements OnInit, OnDestroy {
    constructor(
        private renderer: Renderer2,
        private apiService: ApiHandlerService,
    ) {}

    isLoading: boolean = false;
    profileDeets: Profile | undefined;

    ngOnDestroy(): void {
        this.renderer.removeClass(document.body, 'bg-secondary');
    }
    ngOnInit(): void {
        this.renderer.addClass(document.body, 'bg-secondary');
        this.getProfileInformation();
    }

    getProfileInformation() {
        this.isLoading = true;
        this.apiService.get<Profile>('api/profile/get').subscribe({
            next: (response) => {
                this.profileDeets = response.data;
                this.isLoading = false;
                console.log(response);
            },
            error: (error) => {
                this.isLoading = false;
                console.log(error);
            },
        });
    }
}
