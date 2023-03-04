import {Component, OnInit} from '@angular/core';
import {Order, OrderStatus} from "../../../interfaces/responses/Order";
import {OrderService} from "../../../services/order.service";
import {CurrentAccountService} from "../../../services/currentAccount.service";
import {User} from "../../../interfaces/responses/User";
import {ActivatedRoute} from "@angular/router";
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    DatePipe,
    CurrencyPipe,
  ]
})
export class OrderComponent implements OnInit {

  order?: Order;
  account?: User;

  constructor(
    private orderService: OrderService,
    private currentAccountService: CurrentAccountService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.currentAccountService.currentUser?.subscribe(user => {
      this.account = user;
    });
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        if (this.account?.role === 0) {
          this.orderService.getByIdForUser(id).subscribe(order => {
            if (order) {
              this.order = order;
            }
          });
          return;
        }
        this.orderService.getByOrderId(id).subscribe(order => {
          if (order) {
            this.order = order;
          }
        });
      }
    });
  }

  getAddress() {
    if (this.order?.address) {
      return this.order?.address?.streetName + " " + this.order?.address?.streetNumber + ", " + this.order?.address?.city + ", " + this.order?.address?.country;
    }
    return "";
  }

  displayStatus(status: OrderStatus) {
    return OrderStatus[status];
  }
}
