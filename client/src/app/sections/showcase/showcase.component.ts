import { AfterViewInit, Component } from '@angular/core';
import Initswiper from '../../services/swiper';

@Component({
  selector: 'app-showcase',
  standalone: true,
  imports: [],
  templateUrl: './showcase.component.html',
  styleUrl: './showcase.component.css'
})
export class ShowcaseComponent implements AfterViewInit{

    ngOnInit(): void {}


    ngAfterViewInit(): void {
        Initswiper()
       
    }


}
