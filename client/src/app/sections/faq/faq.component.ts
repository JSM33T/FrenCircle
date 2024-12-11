import { Component } from '@angular/core';
import { faqs } from './faq_data';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-faq',
  standalone: true,
  imports: [NgFor],
  templateUrl: './faq.component.html',
  styleUrl: './faq.component.css'
})
export class FaqComponent {
    texts = {
        faq_data : faqs
    }

}
