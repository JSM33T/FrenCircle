import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-parties',
  imports: [CommonModule],
  templateUrl: './parties.component.html',
  styleUrls: ['./parties.component.scss']
})
export class PartiesComponent {
  parties = [
    {
      name: 'Summer BBQ Party',
      date: 'Jul 25',
      location: 'Central Park',
      host: 'Sarah Johnson',
      attendees: '24 going',
      description: 'Join us for a fun summer BBQ with games, music, and great food!'
    },
    {
      name: 'Game Night',
      date: 'Jul 22',
      location: 'Mike\'s Apartment',
      host: 'Mike Chen',
      attendees: '8 going',
      description: 'Board games, video games, and pizza. What more could you want?'
    },
    {
      name: 'Beach Volleyball',
      date: 'Jul 28',
      location: 'Santa Monica Beach',
      host: 'Alex Rivera',
      attendees: '16 going',
      description: 'Competitive volleyball followed by beachside snacks and drinks.'
    },
    {
      name: 'Movie Marathon',
      date: 'Jul 30',
      location: 'Downtown Cinema',
      host: 'Emma Wilson',
      attendees: '12 going',
      description: 'Back-to-back superhero movies with popcorn and discussion breaks.'
    }
  ];
}
