import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import * as bootstrap from 'bootstrap';
import { ThemeToggleComponent } from '../../ui/theme-toggle/theme-toggle.component';
import { environment } from '../../../../environments/environment';
import { NavbarService } from '../../../services/navbar.service';
import { Subscription } from 'rxjs';

interface UserClaims {
	username: string;
	pfp: string;
	firstname: string;
}

@Component({
	selector: 'app-navbar',
	standalone: true,
	imports: [RouterLink, ThemeToggleComponent],
	templateUrl: './navbar.component.html',
	styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit, OnDestroy {
	user: UserClaims = {
		username: '',
		pfp: `https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o=`,
		firstname: 'guest',
	};
	assets: string = environment.urls.cdnUrl;
	private subscription!: Subscription;

	constructor(private navbarService: NavbarService, private router: Router) {}
	ngOnDestroy(): void {
		this.subscription.unsubscribe();
	}

	ngOnInit(): void {
		this.subscription = this.navbarService.triggerUpdate$.subscribe(() => {
			this.loadLoginState();
		});

		this.loadLoginState();
	}
	logout() {
		localStorage.removeItem('token');
        this.closeModal();
		this.navbarService.triggerNavbarUpdate();
		this.router.navigate(['/']); // Adjust the path as needed
		//window.location.reload();
	}
	isLoggedIn: any;

	loadLoginState() {
		const token = localStorage.getItem('token'); // Retrieve your JWT token from local storage
		if (token) {
			const decodedToken = jwtDecode(token) as any;
			this.user.username = decodedToken.username;
			this.user.firstname = decodedToken.firstname;

			this.user.pfp = decodedToken.avatar;

			this.isLoggedIn = true;
		} else {
			this.user.firstname = 'guest';
			this.user.pfp = `https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o=`;
			this.isLoggedIn = false;
		}
	}

	navToggler() {
		const button = document.querySelector('button.navbar-toggler') as HTMLButtonElement;
		if (button.getAttribute('aria-expanded') === 'true') {
			const bb = document.querySelector('.navbar-toggler') as HTMLButtonElement;
			bb.click();
		}
	}

    closeModal()
    {
      const modals = document.querySelectorAll('.modal.show');
          modals.forEach((modal) => {
              const modalInstance = bootstrap.Modal.getInstance(modal);
              if (modalInstance) {
                  modalInstance.hide();
              }
          });

          // Remove all modal backdrops
          const backdrops = document.querySelectorAll('.modal-backdrop');
          backdrops.forEach((backdrop) => {
              backdrop.remove();
          });

          // Ensure the body doesn't retain modal-specific styles
          document.body.classList.remove('modal-open');
          document.body.style.overflow = '';
          document.body.style.paddingRight = '';
    }
}
