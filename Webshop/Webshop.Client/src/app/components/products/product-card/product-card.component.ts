import {Component, Input, OnInit} from '@angular/core';
import {Product} from "../../../interfaces/responses/Product";
import {CurrencyPipe, NgForOf, NgIf} from "@angular/common";
import {RouterLink} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {ProductVariant} from "../../../interfaces/responses/ProductVariant";
import {ProductBasketComponent} from "../product-basket/product-basket.component";
import {ProductStockComponent} from "../product-stock/product-stock.component";

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss'],
  standalone: true,
  imports: [
    CurrencyPipe,
    RouterLink,
    FormsModule,
    NgForOf,
    NgIf,
    ProductBasketComponent,
    ProductStockComponent
  ]
})
export class ProductCardComponent implements OnInit {
  selectedVariant?: ProductVariant;
  variantId: string = '1';

  @Input() product!: Product;

  constructor() { }

  ngOnInit(): void {
    if (this.product.variants.length == 1) {
      this.selectedVariant = this.product.variants[0];
    }
  }

  updateVariant(){
    this.selectedVariant = this.product.variants.find(x => x.id === this.variantId);
  }

  getStock(): number {
    return this.selectedVariant?.stock ?? this.product.variants
      .map(x => x.stock)
      .reduce((total, value) => total + value, 0);
  }
}
