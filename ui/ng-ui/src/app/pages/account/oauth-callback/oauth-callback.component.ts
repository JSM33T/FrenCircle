import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../../services/auth.service';

@Component({
	selector: 'app-oauth-callback',
	standalone: true,
	imports: [
		CommonModule,
		MatProgressSpinnerModule,
		MatCardModule
	],
	template: `
    <div class="callback-container">
      <mat-card class="callback-card">
        <mat-card-content>
          <div class="callback-content">
            <mat-spinner diameter="40"></mat-spinner>
            <h2>Completing your login...</h2>
            <p>Please wait while we process your Google authentication.</p>
          </div>
        </mat-card-content>
      </mat-card>
    </div>
  `,
	styles: [`
    .callback-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    }

    .callback-card {
      padding: 32px;
      border-radius: 12px;
      box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
    }

    .callback-content {
      text-align: center;

      h2 {
        margin: 24px 0 8px;
        color: #333;
      }

      p {
        margin: 0;
        color: #666;
      }
    }
  `]
})
export class OAuthCallbackComponent implements OnInit {
	private readonly authService = inject(AuthService);
	private readonly router = inject(Router);
	private readonly route = inject(ActivatedRoute);

	ngOnInit(): void {
		this.route.queryParams.subscribe(params => {
			const code = params['code'];
			const state = params['state'];
			const error = params['error'];

			if (error) {
				console.error('OAuth error:', error);
				this.router.navigate(['/account'], {
					queryParams: { error: 'OAuth authentication failed' }
				});
				return;
			}

			if (code) {
				this.authService.handleOAuthCallback(code, state).subscribe({
					next: (response) => {
						// Redirect to home or intended page
						const returnUrl = sessionStorage.getItem('returnUrl') || '/';
						sessionStorage.removeItem('returnUrl');
						this.router.navigate([returnUrl]);
					},
					error: (error) => {
						console.error('OAuth callback error:', error);
						this.router.navigate(['/account'], {
							queryParams: { error: error.message || 'Authentication failed' }
						});
					}
				});
			} else {
				this.router.navigate(['/account'], {
					queryParams: { error: 'No authorization code received' }
				});
			}
		});
	}
}
