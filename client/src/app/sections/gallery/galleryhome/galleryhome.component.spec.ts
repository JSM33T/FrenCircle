import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GalleryhomeComponent } from './galleryhome.component';

describe('GalleryhomeComponent', () => {
  let component: GalleryhomeComponent;
  let fixture: ComponentFixture<GalleryhomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GalleryhomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GalleryhomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
