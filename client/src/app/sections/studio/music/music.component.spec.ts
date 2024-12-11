import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MusicComponent } from './music.component';

describe('MusicComponent', () => {
  let component: MusicComponent;
  let fixture: ComponentFixture<MusicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MusicComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MusicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
