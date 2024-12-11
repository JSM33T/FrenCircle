import { Component } from '@angular/core';
import { GetterService } from '../../services/getters/getter.service';
import { NgFor } from '@angular/common';
import { environment } from '../../../environments/environment';
import { about, title } from './text';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './about.component.html',
  styleUrl: './about.component.css'
})
export class AboutComponent {

  constructor(private getterService: GetterService) {}


  texts : any  = {
    title : title,
    about : about,
  };
  

  ngOnInit(): void {

    
    // this.getterService.getJson(environment.urls.cdnUrl + "data/faqs.json").subscribe((data) => {
    //   this.faqs = data;
    // });
  }


}