import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthenticationService } from '../../core/services/authentication.service';
import { AccountService } from '../../services/account.service';
import { User } from 'src/app/interfaces/responses/User';
import {CurrentAccountService} from "../../services/currentAccount.service";

@Component({
  selector: 'app-navbar-user',
  templateUrl: './navbar-user.component.html',
  styles: []
})
export class NavbarUserComponent implements OnInit {
  user?: User;
  userLabel: string = '';

  constructor(
    private authService: AuthenticationService,
    private accountService: AccountService,
    private router: Router,
    private currentAccountService: CurrentAccountService,
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe(e => {
      if (e instanceof NavigationEnd && this.isLoggedIn() && !this.user) {
        this.getUser();
      }
    });
  }

  private getUser(): void {
    this.currentAccountService.currentUser?.subscribe(user =>  {
      this.user = user;
      this.userLabel = user?.fullName ?? user?.email ?? 'User';
    });
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  logout() {
    this.authService.logout();
  }
}
