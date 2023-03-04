import {Component} from '@angular/core';
import {RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
  imports: [
    RouterLink,
    RouterOutlet,
    RouterLinkActive
  ],
  standalone: true
})
export class AdminComponent {

}
