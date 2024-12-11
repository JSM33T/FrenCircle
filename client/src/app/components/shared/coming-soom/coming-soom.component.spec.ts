import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComingSoomComponent } from './coming-soom.component';

describe('ComingSoomComponent', () => {
  let component: ComingSoomComponent;
  let fixture: ComponentFixture<ComingSoomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComingSoomComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComingSoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
