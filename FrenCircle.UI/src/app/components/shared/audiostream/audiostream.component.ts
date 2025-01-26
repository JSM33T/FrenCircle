import { Component } from '@angular/core';
import { AudioStreamService } from '../../../services/Signalling/audiostream.service';

@Component({
    selector: 'app-audiostream',
    imports: [],
    templateUrl: './audiostream.component.html',
    styleUrl: './audiostream.component.css',
})
export class AudiostreamComponent {
    private audioContext: AudioContext | null = null;
    private mediaStream: MediaStream | null = null;
    private streamerId = 'system-audio-stream';

    constructor(private audioStreamService: AudioStreamService) {}

    ngOnInit() {
        this.audioContext = new AudioContext();
    }

    async startSystemAudioCapture() {
        try {
            const displayMedia = await navigator.mediaDevices.getDisplayMedia({
                video: true, // Optionally set to true if you want to capture video as well
                audio: true, // Capture audio
            });

            this.mediaStream = displayMedia;
            const mediaRecorder = new MediaRecorder(displayMedia, {
                mimeType: 'audio/webm',
            });

            mediaRecorder.ondataavailable = async (event) => {
                if (event.data.size > 0) {
                    const arrayBuffer = await event.data.arrayBuffer();
                    const uint8Array = new Uint8Array(arrayBuffer);
                    this.audioStreamService.sendAudioChunk(
                        this.streamerId,
                        uint8Array,
                    );
                }
            };

            mediaRecorder.start(1000);
            await this.audioStreamService.startStream(this.streamerId);
        } catch (error) {
            console.error('System audio capture error:', error);
        }
    }

    stopSystemAudioCapture() {
        if (this.mediaStream) {
            this.mediaStream.getTracks().forEach((track) => track.stop());
            this.audioStreamService.stopStream(this.streamerId);
            this.mediaStream = null;
        }
    }
}
