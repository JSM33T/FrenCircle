import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttributionsComponent } from './attributions.component';

describe('AttributionsComponent', () => {
  let component: AttributionsComponent;
  let fixture: ComponentFixture<AttributionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AttributionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AttributionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
