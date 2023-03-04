import {Component} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: [],
  providers: [
    HttpClient,
  ],
})
export class AppComponent {
  title = 'Webshop.Client';
}
