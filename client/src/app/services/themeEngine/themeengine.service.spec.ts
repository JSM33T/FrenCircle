import { TestBed } from '@angular/core/testing';
import { ThemeEngineService } from './themeengine.service';

describe('ThemeengineService', () => {
  let service: ThemeEngineService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ThemeEngineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
