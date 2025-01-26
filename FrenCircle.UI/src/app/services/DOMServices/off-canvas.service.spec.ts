import { TestBed } from '@angular/core/testing';

import { OffCanvasService } from './off-canvas.service';

describe('OffCanvasService', () => {
  let service: OffCanvasService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OffCanvasService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
