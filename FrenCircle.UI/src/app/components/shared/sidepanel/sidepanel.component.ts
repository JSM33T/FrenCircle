/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit } from '@angular/core';
// import { Offcanvas } from 'bootstrap';
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';
import { AudioPlayerService } from '../../../services/audioplayer.service';
import { AudioPlayerComponent } from '../audio-player/audio-player.component';

@Component({
    selector: 'app-sidepanel',
    imports: [CdkDrag, CdkDragHandle, AudioPlayerComponent],
    templateUrl: './sidepanel.component.html',
    styleUrl: './sidepanel.component.css',
})
export class SidepanelComponent implements OnInit {
    constructor(private audioService: AudioPlayerService) {}
    ngOnInit(): void {
        setTimeout(() => {
            this.playSong();
        }, 4000);
    }

    playSong() {
        this.audioService.playTrack({
            title: 'Song Title',
            artist: 'Artist Name',
            url: 'https://file-examples.com/storage/fef6bc58a467a23f99bc53c/2017/11/file_example_MP3_700KB.mp3',
        });
    }
}
