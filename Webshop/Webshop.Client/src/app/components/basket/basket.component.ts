import {CurrencyPipe, NgForOf, NgIf} from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Basket } from 'src/app/interfaces/Basket';
import { BasketService } from 'src/app/services/basket.service';
import {AddressComponent} from "../address/address.component";
import {Address} from "../../interfaces/Address";
import {OrderService} from "../../services/order.service";
import {OrderRequest} from "../../interfaces/requests/OrderRequest";
import {AccountService} from "../../services/account.service";
import {BasketCardComponent} from "./basket-card/basket-card.component";
import {AuthenticationService} from "../../core/services/authentication.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styles: [],
  imports: [NgForOf, NgIf, CurrencyPipe, AddressComponent, BasketCardComponent],
  standalone: true,
})
export class BasketComponent implements OnInit {
  basket: Basket = { basketProducts: [] };
  totalBasketPrice: number = 0;
  address: Address = {id: '', streetName: '', streetNumber: '', city: '', zipCode: '', country: ''};

  constructor(
    private basketService: BasketService,
    private orderService: OrderService,
    private accountService: AccountService,
    private authService: AuthenticationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.basketService.currentBasket.subscribe(basket => {
      this.basket = basket;
      this.totalBasketPrice = basket.basketProducts
        .map(bp => bp.productVariant.sellingPrice * bp.quantity)
        .reduce((total, value) => total + value, 0);
    });
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  login() {
    this.router.navigate(['/login']);
  }

  canCheckout(): boolean {
    if (this.basket.basketProducts.length == 0)
      return false;

    return this.isValidAddress() && this.allProductsIsInStock();
  }

  isValidAddress(): boolean {
    return this.address.streetNumber != '' && this.address.streetName != '' && this.address.city != '' && this.address.zipCode != '' && this.address.country != '';
  }

  allProductsIsInStock(): boolean {
    return this.basket.basketProducts.every(bp => bp.productVariant.stock >= bp.quantity);
  }

  checkout() {
    this.accountService.getAccount().subscribe(account => {
      const orderRequest: OrderRequest = {
        id: '00000000-0000-0000-0000-000000000000',
        userId: account.id,
        address: this.address,
        products: this.basketService.getOrderProductRequests()
      }

      this.orderService.create(orderRequest).subscribe(order => {
        this.basketService.clearBasket();

        this.router.navigate(['account/order/' + order.id]);
      });
    });
  }
}
