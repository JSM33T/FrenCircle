import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BreadcrumbsComponent } from '../../components/ui/breadcrumbs/breadcrumbs.component';

@Component({
    selector: 'app-account',
    imports: [RouterLink, BreadcrumbsComponent],
    templateUrl: './account.component.html',
    styleUrl: './account.component.css',
})
export class AccountComponent {}
