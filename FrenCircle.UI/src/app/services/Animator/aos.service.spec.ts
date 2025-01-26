import { TestBed } from '@angular/core/testing';

import { AOSService } from './aos.service';

describe('AOSService', () => {
	let service: AOSService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(AOSService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
