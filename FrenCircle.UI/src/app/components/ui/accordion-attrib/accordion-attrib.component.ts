import { NgFor, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-accordion-attrib',
  imports: [NgFor, NgIf],
  templateUrl: './accordion-attrib.component.html',
  styleUrl: './accordion-attrib.component.css'
})
export class AccordionAttribComponent {
  @Input() data: { field: string; users: any }[] = [];
}
