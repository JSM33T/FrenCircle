import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudiohomeComponent } from './studiohome.component';

describe('StudiohomeComponent', () => {
	let component: StudiohomeComponent;
	let fixture: ComponentFixture<StudiohomeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [StudiohomeComponent],
		}).compileComponents();

		fixture = TestBed.createComponent(StudiohomeComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
