import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { ComingSoomComponent } from '../../../components/shared/coming-soom/coming-soom.component';


@Component({
  selector: 'app-music',
  standalone: true,
  imports: [ComingSoomComponent],
  templateUrl: './music.component.html',
  styleUrl: './music.component.css'
})
export class MusicComponent {
    asset : string = environment.urls.cdnUrl
}
