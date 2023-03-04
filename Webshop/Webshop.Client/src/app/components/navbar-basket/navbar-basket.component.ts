import { Component, OnInit } from '@angular/core';
import { BasketService } from 'src/app/services/basket.service';

@Component({
  selector: 'app-navbar-basket',
  templateUrl: './navbar-basket.component.html',
  styleUrls: ['./navbar-basket.component.scss']
})
export class NavbarBasketComponent implements OnInit {
  basketCount: number = 0;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basketService.currentBasket.subscribe(basket => {
      this.basketCount = basket.basketProducts
        .map(bp => bp.quantity)
        .reduce((total, value) => total + value, 0);
    });
  }
}
