import { TestBed } from '@angular/core/testing';

import { GetterService } from './getter.service';

describe('GetterService', () => {
  let service: GetterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
