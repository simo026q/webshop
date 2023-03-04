import { AuthenticationService } from './services/authentication.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import {AccountService} from "../services/account.service";
import {take} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private accountService: AccountService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
    }
    if (this.authService.isLoggedIn()) {
      this.accountService.getAccount().pipe(take(1)).subscribe(user => {
        if (user.role !== 1 && user.role !== 2) {
          this.router.navigate(['/']);
        }
      });
    }

    return true;
  }
}
