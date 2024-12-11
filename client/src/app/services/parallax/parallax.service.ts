import { Injectable, ElementRef } from '@angular/core';
import Parallax from 'parallax-js';

@Injectable({
  providedIn: 'root',
})
export class ParallaxService {
  private parallaxInstance?: Parallax;

  constructor() {}

  initParallax(element: ElementRef): void {
    this.parallaxInstance = new Parallax(element.nativeElement);
  }

  destroyParallax(): void {
    this.parallaxInstance?.destroy();
    this.parallaxInstance = undefined;
  }
}
