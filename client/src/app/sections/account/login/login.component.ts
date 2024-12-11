import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import * as bootstrap from 'bootstrap';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { GoogleLoginProvider, GoogleSigninButtonModule, SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { HttpService } from '../../../services/HttpServices/http.service';
import { ResponseHandlerService } from '../../../services/HttpServices/response-handler.service';
import { AuthService } from '../../../services/auth.service';
import { APIResponse } from '../../../models/IAPIResponse';
import { NavbarService } from '../../../services/navbar.service';


@Component({
	selector: 'app-login',
	standalone: true,
	imports: [ReactiveFormsModule, RouterModule, GoogleSigninButtonModule],
	templateUrl: './login.component.html',
	styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
	isLoading: boolean = false;
	loginForm!: FormGroup;
	paramUsername: string = '';

	constructor(
		private httpService: HttpService,
		private fb: FormBuilder,
		private router: Router,
		private responseHandler: ResponseHandlerService,
		private route: ActivatedRoute,
		private authService: AuthService,
		private socialAuthService: SocialAuthService,
        private navbarService: NavbarService
	) {
		this.loginForm = this.fb.group({
			username: new FormControl(''),
			password: new FormControl(''),
		});
	}
	ngOnInit(): void {
		this.paramUsername = this.route.snapshot.queryParamMap.get('username') ?? '';
		if (this.paramUsername != '') {
			this.loginForm.patchValue({
				username: this.paramUsername,
			});
		}

		this.socialAuthService.authState.subscribe((user: SocialUser) => {
			if (user) {
				console.log('User logged in:', user);
				this.handleLogin(user.idToken);
			}
		});
	}

	signInWithGoogle(): void {
		this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
	}

	handleLogin(idToken: string) {
		this.authService.googleLogin(idToken).subscribe({
			next: (response) => {
				console.log('Login successful', response);
				this.isLoading = false;
				if (response.status == 200) {
					console.log(response.data.token);
					localStorage.setItem('token', response.data.token);
					//this.router.navigate(['/']);
                    this.onLoginSuccess();
				}
			},
			error: (error) => {
				console.error('Login failed', error);
				// Handle login error
			},
		});
	}

	onSubmit(): void {
		this.isLoading = true;
		const response$: Observable<APIResponse<any>> = this.httpService.post('api/account/login', this.loginForm.value);

		this.responseHandler.handleResponse(response$, true).subscribe({
			next: (response) => {
				this.isLoading = false;
				if (response.status == 200) {
					localStorage.setItem('token', response.data.token);
					//this.router.navigate(['/']);
                    this.onLoginSuccess();
				}
			},
			error: (error) => {
				console.log(error.error);
				this.isLoading = false;
			},
		});
	}

	togglePasswordVisibility(input: HTMLInputElement) {
		input.type = input.type === 'password' ? 'text' : 'password';
	}
    
    
    onLoginSuccess() {
        this.navbarService.triggerNavbarUpdate();
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/';
        //this.closeModal();
        this.router.navigateByUrl(decodeURIComponent(returnUrl));  // Decode it just in case
      }

     
}
