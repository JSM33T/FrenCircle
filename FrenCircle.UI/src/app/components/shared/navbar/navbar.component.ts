/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ThemeToggleComponent } from '../theme-toggle/theme-toggle.component';
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';
import { jwtDecode } from 'jwt-decode';
import { ModalService } from '../../../services/DOMServices/modal.service';

interface UserClaims {
    username: string;
    pfp: string;
    firstname: string;
    authMode: string;
}

@Component({
    selector: 'app-navbar',
    imports: [RouterLink, ThemeToggleComponent, CdkDrag, CdkDragHandle],
    templateUrl: './navbar.component.html',
    styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
    constructor(
        private router: Router,
        private mdlService: ModalService,
    ) {}

    user: UserClaims = {
        username: '',
        pfp: 'https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg',
        firstname: '',
        authMode: 'email',
    };
    isLoggedIn: boolean = false;

    ngOnInit(): void {
        const token = localStorage.getItem('token'); // Retrieve your JWT token from local storage
        if (token) {
            const decodedToken = jwtDecode(token) as any;
            this.user.username = decodedToken.unique_name;
            this.isLoggedIn = true;
        } else {
            this.isLoggedIn = false;
        }
    }

    // triggerLogout() {
    //     this.mdlService.openModal('mdl_logout');
    // }

    logout() {
        localStorage.removeItem('token');
        window.location.reload();
        //this.reloadComponent(true, '/');
    }

    redirectToLogin() {
        const returnUrl = this.router.url; // Capture current route
        this.router.navigate(['/account'], {
            queryParams: { returnUrl },
        }); // Pass it as a param
    }
}
