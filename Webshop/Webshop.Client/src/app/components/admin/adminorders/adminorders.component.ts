import {Component, OnInit} from '@angular/core';
import {RouterLink} from "@angular/router";
import {DatePipe, NgForOf} from "@angular/common";
import {PaginationComponent} from "../../pagination/pagination.component";
import {Order, OrderStatus} from "../../../interfaces/responses/Order";
import {OrderService} from "../../../services/order.service";
import {CurrentAccountService} from "../../../services/currentAccount.service";
import {take} from "rxjs";
import {FormsModule} from "@angular/forms";

@Component({
    selector: 'app-adminorders',
    templateUrl: './adminorders.component.html',
    styleUrls: ['./adminorders.component.scss'],
    imports: [
        RouterLink,
        NgForOf,
        PaginationComponent,
        FormsModule,
        DatePipe
    ],
    standalone: true
})
export class AdminordersComponent implements OnInit {
  orders: Order[] = [];
  totalItems = 0;
  currentPage = 1;
  productsPerPage = 12;
  constructor(private orderService: OrderService) {}

  ngOnInit() {
    this.getOrders();
  }

  changeStatus(order: Order, status: OrderStatus) {
    order.status = status;
    this.orderService.changeStatus(order.id, order.status).pipe(take(1)).subscribe(x => {
      this.getOrders();
    });
  }

  getStatuses(): [string, number][] {
    return Object.entries(OrderStatus)
      .filter(([key, value]) => typeof value === 'number')
      .map(([key, value]) => [key, Number(value)]);
  }

  displayStatus(status: OrderStatus) {
    return OrderStatus[status];
  }

  changePage(page: number) {
    this.currentPage = page;
    this.getOrders();
  }

  changeSize(size: number) {
    this.productsPerPage = size;
    this.getOrders();
  }

  private getOrders() {
    this.orderService.getAll(this.currentPage, this.productsPerPage).pipe(take(1)).subscribe(orders => {
      this.totalItems = Number(orders.headers.get('total-count'));
      this.orders = orders.body!;
    });
  }
}
