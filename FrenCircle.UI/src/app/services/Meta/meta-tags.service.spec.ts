import { TestBed } from '@angular/core/testing';

import { MetaTagsService } from './meta-tags.service';

describe('MetaTagsService', () => {
  let service: MetaTagsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MetaTagsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
