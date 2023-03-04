import {Component, OnInit} from '@angular/core';
import {Product} from "../../../interfaces/responses/Product";
import {ActivatedRoute} from "@angular/router";
import {ProductService} from "../../../services/product.service";
import {CurrencyPipe, NgForOf, NgIf} from "@angular/common";
import {ProductVariant} from "../../../interfaces/responses/ProductVariant";
import {FormsModule} from "@angular/forms";
import {take} from "rxjs";
import {ProductBasketComponent} from "../product-basket/product-basket.component";
import {ProductStockComponent} from "../product-stock/product-stock.component";

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styles: [],
  imports: [
    NgIf,
    FormsModule,
    NgForOf,
    CurrencyPipe,
    ProductBasketComponent,
    ProductStockComponent
  ],
  standalone: true
})

export class ProductComponent implements OnInit {
  product: Product = {} as Product;
  productId? : string;
  selectedVariant?: ProductVariant;
  variantId: string = '';
  quantity: number = 1;
  displayPrice?: number;
  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.productId = params.get('id')!;
      // fetch the product details from your data service using the id
      this.productService.get(this.productId).pipe(take(1)).subscribe(product => {
        this.product = product;
        this.variantId = this.product.variants[0].id;
        this.selectedVariant = this.product.variants[0];
        this.displayPrice = this.selectedVariant?.sellingPrice;
      });
    });
  }

  updateVariant() {
    this.selectedVariant = this.product.variants.find(x => x.id === this.variantId);
    this.displayPrice = this.selectedVariant?.sellingPrice;
  }

  setQuantity() {
    this.quantity = Math.max(this.quantity, 1);
  }

  getStock(): number {
    return this.selectedVariant?.stock ?? this.product.variants
      .map(x => x.stock)
      .reduce((total, value) => total + value, 0);
  }
}
