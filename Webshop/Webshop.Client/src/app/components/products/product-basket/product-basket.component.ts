import {Component, Input} from '@angular/core';
import { CommonModule } from '@angular/common';
import {BasketService} from "../../../services/basket.service";
import {Product} from "../../../interfaces/responses/Product";
import {ProductVariant} from "../../../interfaces/responses/ProductVariant";

@Component({
  selector: 'app-product-basket',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-basket.component.html',
  styles: []
})
export class ProductBasketComponent {
  @Input() product!: Product;
  @Input() productVariant?: ProductVariant;
  @Input() quantity = 1;

  constructor(private basketService: BasketService) { }

  addToCart() {
    // add to cart
    if (this.productVariant) {
      this.basketService.addProductVariant(this.product, this.productVariant, this.quantity);
    }
  }

  removeFromCart() {
    // remove from cart
    if (this.productVariant) {
      this.basketService.removeProductVariant(this.productVariant.id);
    }
  }



  getCartQuantity(): number {
    if (this.productVariant) {
      return this.basketService.getProductVariant(this.productVariant.id);
    }

    return 0;
  }
}
