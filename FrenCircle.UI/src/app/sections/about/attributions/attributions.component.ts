import { Component } from '@angular/core';
import { BreadcrumbsComponent } from '../../../components/ui/breadcrumbs/breadcrumbs.component';
import { AccordionAttribComponent } from "../../../components/ui/accordion-attrib/accordion-attrib.component";
import { dt_attributions } from '../../../data/dt_attributions';

@Component({
  selector: 'app-attributions',
  imports: [BreadcrumbsComponent, AccordionAttribComponent],
  templateUrl: './attributions.component.html',
  styleUrl: './attributions.component.css'
})
export class AttributionsComponent {
  attribdata = dt_attributions;
}
