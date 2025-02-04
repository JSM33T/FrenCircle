/* eslint-disable @typescript-eslint/no-explicit-any */
import { ChangeDetectorRef, Component } from '@angular/core';
import { AudioPlayerService } from '../../../services/audioplayer.service';
import { DecimalPipe, NgIf } from '@angular/common';

@Component({
    selector: 'app-audio-player',
    imports: [NgIf],
    templateUrl: './audio-player.component.html',
    styleUrl: './audio-player.component.css',
    providers: [DecimalPipe],
})
export class AudioPlayerComponent {
    constructor(
        public audioService: AudioPlayerService,
        private cdRef: ChangeDetectorRef,
        private decimalPipe: DecimalPipe,
    ) {}

    togglePlayPause() {
        this.audioService.togglePlayPause();
        this.cdRef.detectChanges();
    }

    formatTime(value: number): string {
        return this.decimalPipe.transform(value, '1.0-0') || '0';
    }

    seek(event: any) {
        this.audioService.seek(event.target.value);
    }
}
