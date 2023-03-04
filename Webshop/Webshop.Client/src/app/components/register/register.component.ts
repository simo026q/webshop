import {Component} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {AuthenticationService} from "../../core/services/authentication.service";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [
    RouterLink,
    FormsModule
  ]
})
export class RegisterComponent {
  fullName?: string;
  email?: string;
  password?: string;

  constructor(private authenticationService: AuthenticationService) {}


  public onSubmit() {
    this.authenticationService.register(
      this.fullName!,
      this.email!,
      this.password!
    );
  }
}
