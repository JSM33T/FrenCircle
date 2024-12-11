import { Component } from '@angular/core';
import { ComingSoomComponent } from "../../../components/shared/coming-soom/coming-soom.component";

@Component({
  selector: 'app-bloghome',
  standalone: true,
  imports: [ComingSoomComponent],
  templateUrl: './bloghome.component.html',
  styleUrl: './bloghome.component.css'
})
export class BloghomeComponent {

}
