import { TestBed } from '@angular/core/testing';

import { ParallaxService } from '../parallax/parallax.service';

describe('ParallaxService', () => {
  let service: ParallaxService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ParallaxService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
