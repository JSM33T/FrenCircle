import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-parties',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatDividerModule
  ],
  templateUrl: './parties.component.html',
  styleUrls: ['./parties.component.scss']
})
export class PartiesComponent {
  parties = [
    {
      id: 1,
      name: 'Summer BBQ Party',
      date: 'Jul 25',
      time: '6:00 PM',
      location: 'Central Park',
      host: 'Sarah Johnson',
      attendees: 24,
      maxAttendees: 30,
      description: 'Join us for a fun summer BBQ with games, music, and great food!',
      tags: ['BBQ', 'Outdoor', 'Music'],
      image: 'https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=400&h=200&fit=crop'
    },
    {
      id: 2,
      name: 'Game Night',
      date: 'Jul 22',
      time: '7:30 PM',
      location: 'Mike\'s Apartment',
      host: 'Mike Chen',
      attendees: 8,
      maxAttendees: 12,
      description: 'Board games, video games, and pizza. What more could you want?',
      tags: ['Games', 'Indoor', 'Casual'],
      image: 'https://images.unsplash.com/photo-1606092195730-5d7b9af1efc5?w=400&h=200&fit=crop'
    },
    {
      id: 3,
      name: 'Beach Volleyball',
      date: 'Jul 28',
      time: '2:00 PM',
      location: 'Santa Monica Beach',
      host: 'Alex Rivera',
      attendees: 16,
      maxAttendees: 20,
      description: 'Competitive volleyball followed by beachside snacks and drinks.',
      tags: ['Sports', 'Beach', 'Active'],
      image: 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=400&h=200&fit=crop'
    },
    {
      id: 4,
      name: 'Movie Marathon',
      date: 'Jul 30',
      time: '1:00 PM',
      location: 'Downtown Cinema',
      host: 'Emma Wilson',
      attendees: 12,
      maxAttendees: 15,
      description: 'Back-to-back superhero movies with popcorn and discussion breaks.',
      tags: ['Movies', 'Entertainment', 'Indoor'],
      image: 'https://images.unsplash.com/photo-1489599843821-4e3d6a00bc90?w=400&h=200&fit=crop'
    }
  ];

  onJoinParty(partyId: number): void {
    console.log('Joining party:', partyId);
    // Implement join party logic
  }

  onViewDetails(partyId: number): void {
    console.log('Viewing party details:', partyId);
    // Implement view details logic
  }

  trackByPartyId(index: number, party: any): number {
    return party.id;
  }
}
