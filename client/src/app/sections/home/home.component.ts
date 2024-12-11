import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { RouterLink } from '@angular/router';
import { HomeParallaxComponent } from "../../components/ui/home-parallax/home-parallax.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, HomeParallaxComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  ngOnInit(): void {
  }

  homeAssets : string = environment.urls.cdnUrl + "/assets/"

}
