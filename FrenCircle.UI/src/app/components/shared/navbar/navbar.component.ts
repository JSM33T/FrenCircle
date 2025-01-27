import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ThemeToggleComponent } from '../theme-toggle/theme-toggle.component';
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';

@Component({
    selector: 'app-navbar',
    imports: [RouterLink, ThemeToggleComponent, CdkDrag, CdkDragHandle],
    templateUrl: './navbar.component.html',
    styleUrl: './navbar.component.css',
})
export class NavbarComponent {}
