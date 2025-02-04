import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class AudioPlayerService {
    private audio = new Audio();
    currentTrack: { title: string; artist: string; url: string } | null = null;
    isPlaying = false;
    currentTime = 0;
    duration = 0;

    constructor() {
        this.audio.addEventListener('timeupdate', () => {
            this.currentTime = this.audio.currentTime;
            this.duration = this.audio.duration || 0;
        });
    }

    playTrack(track: { title: string; artist: string; url: string }) {
        this.currentTrack = track;
        this.audio.src = track.url;
        this.audio.load();
        this.audio.play();
        this.isPlaying = true;

        this.audio.addEventListener('loadedmetadata', () => {
            this.duration = this.audio.duration;
        });
    }

    togglePlayPause() {
        if (this.isPlaying) {
            this.audio.pause();
        } else {
            this.audio.play();
        }
        this.isPlaying = !this.isPlaying;
    }

    stop() {
        this.audio.pause();
        this.audio.currentTime = 0;
        this.isPlaying = false;
    }

    seek(value: number) {
        this.audio.currentTime = value;
    }
}
