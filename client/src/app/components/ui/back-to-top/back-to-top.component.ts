import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-back-to-top',
  standalone: true,
  imports: [],
  templateUrl: './back-to-top.component.html',
  styleUrl: './back-to-top.component.css'
})
export class BackToTopComponent implements OnInit {


  ngOnInit() {
    const button = document.querySelector('.btn-scroll-top');
    const scrollOffset = 150;

    if (button == null) return;

    const offsetFromTop = parseInt(scrollOffset.toString(), 10);
    const progress = button.querySelector('svg circle') as SVGCircleElement;
    const length = progress.getTotalLength();

    progress.style.strokeDasharray = length.toString();
    progress.style.strokeDashoffset = length.toString();

    const showProgress = () => {
      const scrollPercent = (document.body.scrollTop + document.documentElement.scrollTop) / (document.documentElement.scrollHeight - document.documentElement.clientHeight);
      const draw = length * scrollPercent;
      progress.style.strokeDashoffset = (length - draw).toString();
    };

    window.addEventListener('scroll', (e: Event) => {
      const currentTarget = e.currentTarget as Window;
      if (currentTarget.pageYOffset > offsetFromTop) {
        button.classList.add('show');
      } else {
        button.classList.remove('show');
      }

      showProgress();
    });

    // Add click event listener to scroll to top
    button.addEventListener('click', (e: Event) => {
      e.preventDefault();
      window.scrollTo({
        top: 0,
        behavior: 'smooth',
      });
    });
  }



}
