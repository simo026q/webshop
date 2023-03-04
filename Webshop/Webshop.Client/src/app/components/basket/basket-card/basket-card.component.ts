import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Basket} from "../../../interfaces/Basket";
import {BasketService} from "../../../services/basket.service";
import {Product} from "../../../interfaces/responses/Product";
import {ProductVariant} from "../../../interfaces/responses/ProductVariant";

@Component({
  selector: 'app-basket-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './basket-card.component.html',
  styles: [
  ]
})
export class BasketCardComponent {
  basket: Basket = { basketProducts: [] };
  totalBasketPrice: number = 0;

  constructor(
    private basketService: BasketService
  ) {}

  ngOnInit(): void {
    this.basketService.currentBasket.subscribe(basket => {
      this.basket = basket;
      this.totalBasketPrice = basket.basketProducts
        .map(bp => bp.productVariant.sellingPrice * bp.quantity)
        .reduce((total, value) => total + value, 0);
    });
  }

  removeProductVariant(productVariantId: string) {
    this.basketService.removeProductVariant(productVariantId);
  }

  removeAllProductVariants(productVariantId: string) {
    this.basketService.removeAllProductVariants(productVariantId);
  }

  clearBasket() {
    this.basketService.clearBasket();
  }

  addProductVariant(product: Product, productVariant: ProductVariant) {
    this.basketService.addProductVariant(product, productVariant);
  }
}
