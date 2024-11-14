import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CtaAComponent } from './cta-a.component';

describe('CtaAComponent', () => {
  let component: CtaAComponent;
  let fixture: ComponentFixture<CtaAComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CtaAComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CtaAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
