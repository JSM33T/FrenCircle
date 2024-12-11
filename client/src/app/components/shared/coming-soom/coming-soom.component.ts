import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-coming-soom',
  standalone: true,
  imports: [],
  templateUrl: './coming-soom.component.html',
  styleUrl: './coming-soom.component.css'
})
export class ComingSoomComponent {
assets : string = environment.urls.cdnUrl + "/assets/graphics/images/coming_soon"
}
