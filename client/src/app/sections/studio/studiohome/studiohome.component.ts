import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { ComingSoomComponent } from "../../../components/shared/coming-soom/coming-soom.component";

@Component({
  selector: 'app-studiohome',
  standalone: true,
  imports: [ComingSoomComponent],
  templateUrl: './studiohome.component.html',
  styleUrl: './studiohome.component.css'
})
export class StudiohomeComponent {
    asset : string = environment.urls.cdnUrl
}
