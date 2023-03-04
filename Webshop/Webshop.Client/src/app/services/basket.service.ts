import {Injectable} from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Basket } from '../interfaces/Basket';
import { ProductVariant } from '../interfaces/responses/ProductVariant';
import { Product } from '../interfaces/responses/Product';
import {OrderProductRequest} from "../interfaces/requests/OrderProductRequest";

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private basketSource = new BehaviorSubject<Basket>({ basketProducts: [] });
  currentBasket = this.basketSource.asObservable();

  constructor() {
    const basketJson = localStorage.getItem('basket');

    if (basketJson) {
      this.basketSource.next(JSON.parse(basketJson) as Basket);
    }
  }

  private saveBasket(basket: Basket) {
    localStorage.setItem('basket', JSON.stringify(basket));
    this.basketSource.next(basket);
  }

  addProductVariant(product: Product, productVariant: ProductVariant, quantity?: number) {
    let basket = this.basketSource.getValue();

    let basketProduct = basket.basketProducts.find(bp => bp.productVariant.id === productVariant.id);

    if (!quantity) {
      quantity = 1;
    }
    if (basketProduct) {
      basketProduct.quantity = basketProduct.quantity + quantity;
    } else {
      basket.basketProducts.push({ product, productVariant, quantity: quantity });
    }

    this.saveBasket(basket);
  }

  getProductVariant(productVariantId: string): number {
    let basket = this.basketSource.getValue();

    let basketProduct = basket.basketProducts.find(bp => bp.productVariant.id === productVariantId);

    return basketProduct?.quantity ?? 0;
  }

  removeProductVariant(productVariantId: string) {
    let basket = this.basketSource.getValue();

    let basketProduct = basket.basketProducts.find(bp => bp.productVariant.id === productVariantId);

    if (basketProduct) {
      if (basketProduct.quantity > 1) {
        basketProduct.quantity--;
      } else {
        basket.basketProducts = basket.basketProducts.filter(bp => bp.productVariant.id !== productVariantId);
      }
    }

    this.saveBasket(basket);
  }

  removeAllProductVariants(productVariantId: string) {
    let basket = this.basketSource.getValue();

    basket.basketProducts = basket.basketProducts.filter(bp => bp.productVariant.id !== productVariantId);

    this.saveBasket(basket);
  }

  clearBasket() {
    this.saveBasket({ basketProducts: [] });
  }

  getOrderProductRequests(): OrderProductRequest[] {
    let basket = this.basketSource.getValue();

    return basket.basketProducts.map(bp => {
      return {
        orderId: '00000000-0000-0000-0000-000000000000',
        productVariantId: bp.productVariant.id,
        quantity: bp.quantity
      }
    });
  }
}
