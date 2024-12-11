import { Component, ElementRef, ViewChild } from '@angular/core';
import { ParallaxService } from '../../../services/parallax/parallax.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-home-parallax',
  standalone: true,
  imports: [],
  templateUrl: './home-parallax.component.html',
  styleUrl: './home-parallax.component.css'
})
export class HomeParallaxComponent {

  homeAssets : string = environment.urls.cdnUrl + "/assets/graphics/images/homeparallax";
  @ViewChild('parallaxContainer', { static: true }) parallaxContainer!: ElementRef;

  constructor(private parallaxService: ParallaxService) {}

  
  ngOnInit(): void {
    this.parallaxService.initParallax(this.parallaxContainer);
  }

  ngOnDestroy(): void {
    this.parallaxService.destroyParallax();
  }
}
