import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccordionAttribComponent } from './accordion-attrib.component';

describe('AccordionAttribComponent', () => {
  let component: AccordionAttribComponent;
  let fixture: ComponentFixture<AccordionAttribComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccordionAttribComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AccordionAttribComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
