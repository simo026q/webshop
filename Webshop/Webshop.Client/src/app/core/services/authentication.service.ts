import {Injectable, Injector} from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationClient } from '../clients/authenticationClient';
import { AuthRequest } from "../../interfaces/requests/AuthRequest";
import { User } from "../../interfaces/responses/User";
import { AuthResponse } from "../../interfaces/responses/AuthResponse";
import {CurrentAccountService} from "../../services/currentAccount.service";

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private tokenKey = "token";

  constructor(
    private authenticationClient: AuthenticationClient,
    private router: Router,
    private injector: Injector,
  ) {}

  public login(authRequest: AuthRequest): void {
    this.authenticationClient.login(authRequest).subscribe((auth: AuthResponse) => {
      if (auth.success && auth.token != null) {
        localStorage.setItem(this.tokenKey, auth.token!);
        const currentAccountService = this.injector.get<CurrentAccountService>(CurrentAccountService);
        currentAccountService.onLogin();
        this.router.navigate(['/']).then(() => window.location.reload());
      }
      else {
        window.alert("Invalid credentials");
      }
    });
  }

  public register(username: string, email: string, password: string): void {
    this.authenticationClient
      .register(username, email, password)
      .subscribe((response: User) => {
        if (response) {
          this.router.navigate(['/login']).then();
        }
      });
  }

  public logout() {
    const currentAccountService = this.injector.get<CurrentAccountService>(CurrentAccountService);
    currentAccountService.onLogout();
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/']).then();
  }

  public isLoggedIn(): boolean {
    let token = this.getToken();

    return token !== null && token != "";
  }

  public getToken() {
    return localStorage.getItem(this.tokenKey);
  }
}
