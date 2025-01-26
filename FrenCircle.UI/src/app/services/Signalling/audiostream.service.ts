import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root',
})
export class AudioStreamService {
    private hubConnection: signalR.HubConnection;

    constructor() {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(environment.urls.apiUrl + '/audioStreamHub')
            .build();

        this.setupListeners();
    }

    private setupListeners() {
        this.hubConnection.on('ReceiveAudioChunk', (audioChunk: Uint8Array) => {
            this.playAudioChunk(audioChunk);
        });
    }

    private playAudioChunk(chunk: Uint8Array) {
        // Implement audio playback logic
        const audioContext = new AudioContext();
        const source = audioContext.createBufferSource();
        audioContext.decodeAudioData(chunk.buffer, (buffer) => {
            source.buffer = buffer;
            source.connect(audioContext.destination);
            source.start(0);
        });
    }

    async startStream(streamerId: string) {
        await this.hubConnection.start();
        await this.hubConnection.invoke('StartStream', streamerId);
    }

    async stopStream(streamerId: string) {
        await this.hubConnection.invoke('StopStream', streamerId);
        await this.hubConnection.stop();
    }

    async sendAudioChunk(streamerId: string, audioChunk: Uint8Array) {
        await this.hubConnection.invoke(
            'SendAudioChunk',
            streamerId,
            audioChunk,
        );
    }
}
