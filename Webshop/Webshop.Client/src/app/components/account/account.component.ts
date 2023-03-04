import {Component, OnInit} from '@angular/core';
import {AccountService} from "../../services/account.service";
import {User} from "../../interfaces/responses/User";
import {take} from "rxjs";
import {FormsModule} from "@angular/forms";
import {Router, RouterLink} from "@angular/router";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthenticationService} from "../../core/services/authentication.service";
import {CurrentAccountService} from "../../services/currentAccount.service";
import {Order} from "../../interfaces/responses/Order";
import {OrderService} from "../../services/order.service";
import {OrderStatus} from "../../interfaces/responses/Order";


@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
  standalone: true,
  providers: [AccountService],
  imports: [
    FormsModule,
    NgIf,
    RouterLink,
    NgForOf,
    DatePipe
  ]
})
export class AccountComponent implements OnInit {
  account?: User;

  accountOrders?: Order[];

  constructor(
    public accountService: AccountService,
    private router: Router,
    private authService: AuthenticationService,
    private currentAccountService: CurrentAccountService,
    private orderService: OrderService
  ) {

  }

  ngOnInit(): void {
    this.currentAccountService.currentUser?.subscribe(user => {
      this.account = user;
    });
    this.orderService.getAllByUser(1, 200).pipe(take(1)).subscribe(orders => {
      if (orders.body?.length !== 0 && orders.body !== undefined) {
        this.accountOrders = orders.body!;
      }
    });
  }

  displayStatus(status: OrderStatus) {
    return OrderStatus[status];
  }

  updateAccount() {
    this.accountService.updateAccount({
      id: this.account!.id,
      addressId: this.account!.addressId,
      fullName: this.account!.fullName,
      isActive: this.account!.isActive
    }).pipe(take(1)).subscribe(account => this.account = account);
    location.reload();
  }

  showAdmin(): boolean {
    if (this.account === undefined || this.account === null) {
      return false
    }
    return this.account.role === 2 || this.account.role === 1;
  }

  deleteAccount() {
    this.accountService.deleteAccount().pipe(take(1)).subscribe();
    this.authService.logout()
  }
}
