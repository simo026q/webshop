import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable, take} from 'rxjs';
import { User } from '../interfaces/responses/User';
import {AuthenticationService} from "../core/services/authentication.service";
import {AccountService} from "./account.service";

@Injectable({
  providedIn: 'root'
})
export class CurrentAccountService {
  private userSource = new BehaviorSubject<User>({
    addressId: "",
    createdAt: "",
    email: "",
    fullName: "",
    id: "",
    isActive: false,
    role: 0
  });
  currentUser: Observable<User> | undefined = undefined;

  constructor(private authService: AuthenticationService, private accountService: AccountService) {
    if (this.authService.isLoggedIn()) {
      this.accountService.getAccount().pipe(take(1)).subscribe(user => {
        this.saveUser(user)
      });
    }
  }

  private saveUser(user: User) {
    this.userSource.next(user);
    this.currentUser = this.userSource.asObservable();
  }

  onLogout() {
    this.currentUser = undefined;
  }

  onLogin() {
    this.accountService.getAccount().pipe(take(1)).subscribe(user => {
      this.saveUser(user)
    });
  }
}
