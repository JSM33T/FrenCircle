import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AudiostreamComponent } from './audiostream.component';

describe('AudiostreamComponent', () => {
  let component: AudiostreamComponent;
  let fixture: ComponentFixture<AudiostreamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AudiostreamComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AudiostreamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
