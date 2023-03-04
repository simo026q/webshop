import { Component } from '@angular/core';

@Component({
  selector: 'app-page-not-found',
  template: `
    <div class="container">
      <h1 class="text-center">404 Page Not Found</h1>
    </div>
  `,
  styles: [`
    .container {
      margin-top: 50px;
    }
  `]
})
export class PageNotFoundComponent { }
