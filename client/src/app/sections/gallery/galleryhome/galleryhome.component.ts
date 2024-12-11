import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { ComingSoomComponent } from "../../../components/shared/coming-soom/coming-soom.component";

@Component({
  selector: 'app-galleryhome',
  standalone: true,
  imports: [ComingSoomComponent],
  templateUrl: './galleryhome.component.html',
  styleUrl: './galleryhome.component.css'
})
export class GalleryhomeComponent {
    asset : string = environment.urls.cdnUrl
}
