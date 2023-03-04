import {Component} from '@angular/core';
import {AuthenticationService} from "../../core/services/authentication.service";
import {RouterLink} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {AuthRequest} from "../../interfaces/requests/AuthRequest";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    RouterLink,
    FormsModule
  ]
})
export class LoginComponent {
  authRequest: AuthRequest = {
    email: '',
    password: ''
  };

  constructor(private authenticationService: AuthenticationService) {}


  public onSubmit() {
    this.authenticationService.login(this.authRequest!);
  }
}
