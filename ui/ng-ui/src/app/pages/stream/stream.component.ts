import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-stream',
  imports: [CommonModule],
  templateUrl: './stream.component.html',
  styleUrls: ['./stream.component.scss']
})
export class StreamComponent {
  streams = [
    { title: 'Gaming Night Live', streamer: 'GamerPro123', viewers: '1.2K' },
    { title: 'Music & Chill', streamer: 'MusicLover', viewers: '856' },
    { title: 'Art Creation Stream', streamer: 'ArtistLife', viewers: '432' },
    { title: 'Coding Tutorial', streamer: 'DevMaster', viewers: '2.1K' },
    { title: 'Fitness Workout', streamer: 'FitnessFan', viewers: '678' },
    { title: 'Cooking Show', streamer: 'ChefExpert', viewers: '945' }
  ];
}
