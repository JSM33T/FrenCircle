import { AfterViewChecked, AfterViewInit, Component, OnInit } from '@angular/core';
import Initswiper from '../../services/swiper';
import { HttpService } from '../../services/HttpServices/http.service';
import { MediaItem } from './IMediaCollection';
import { catchError, map } from 'rxjs/operators';
import { NgFor, NgIf } from '@angular/common';

@Component({
	selector: 'app-studio',
	standalone: true,
	imports: [NgFor,NgIf],
	templateUrl: './studio.component.html',
	styleUrl: './studio.component.css',
})
export class StudioComponent implements OnInit {
	stuff: MediaItem[] = [];
	constructor(private httpService: HttpService) {}

	ngOnInit(): void {
		Initswiper();
		this.loadStuff();
	}

	loadStuff(): void {
		this.httpService
			.readJsonFile<MediaItem[]>('https://localhost:7254/content/studio/browse.json')
			.pipe(
				map((data) => data as MediaItem[]), // Transform the data to the expected type
				catchError((error) => {
					console.error('Error loading media items:', error);
					return [];
				})
			)
			.subscribe((data: MediaItem[]) => {
				this.stuff = data; // Assign the loaded data to the 'stuff' array
			});
	}
}
