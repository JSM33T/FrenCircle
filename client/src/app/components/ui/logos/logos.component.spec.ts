import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogosComponent } from './logos.component';

describe('LogosComponent', () => {
  let component: LogosComponent;
  let fixture: ComponentFixture<LogosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LogosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LogosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
