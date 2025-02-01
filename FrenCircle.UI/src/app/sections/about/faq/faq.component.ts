import { Component } from '@angular/core';
import { BreadcrumbsComponent } from '../../../components/ui/breadcrumbs/breadcrumbs.component';
import { DATA_FAQ } from '../../../data/dt_faqs';
import { AccordionComponent } from '../../../components/ui/accordion/accordion.component';

@Component({
  selector: 'app-faq',
  imports: [BreadcrumbsComponent, AccordionComponent],
  templateUrl: './faq.component.html',
  styleUrl: './faq.component.css'
})
export class FaqComponent {

  frencircleFAQs = DATA_FAQ

}
