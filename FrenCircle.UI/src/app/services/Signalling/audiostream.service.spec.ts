import { TestBed } from '@angular/core/testing';

import { AudiostreamService } from './audiostream.service';

describe('AudiostreamService', () => {
  let service: AudiostreamService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AudiostreamService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
