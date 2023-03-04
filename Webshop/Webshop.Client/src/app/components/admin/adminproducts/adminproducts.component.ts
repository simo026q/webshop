import {Component, OnInit} from '@angular/core';
import { RouterLink } from "@angular/router";
import {DatePipe, NgClass, NgForOf} from "@angular/common";
import { Product } from "../../../interfaces/responses/Product";
import { ProductService } from "../../../services/product.service";
import {take} from "rxjs";
import {PaginationComponent} from "../../pagination/pagination.component";

@Component({
    selector: 'app-adminproducts',
    templateUrl: './adminproducts.component.html',
    styleUrls: ['./adminproducts.component.scss'],
    imports: [
        RouterLink,
        NgForOf,
        NgClass,
        PaginationComponent,
        DatePipe
    ],
    standalone: true
})
export class AdminproductsComponent implements OnInit {
    products: Product[] = [];

    totalItems = 0;
    currentPage = 1;
    productsPerPage = 12;
    constructor(private productService: ProductService) { }
    ngOnInit(): void {
      this.updateProducts();
    }
    enableOrDisable(product: Product) {
      if (product.isActive) {
        this.productService.disable(product.id).pipe(take(1)).subscribe();
      } else {
        this.productService.activate(product.id).pipe(take(1)).subscribe();
      }
      product.isActive = !product.isActive;
    }

    getLabel(product: Product) {
      if (product.isActive) {
        return 'Disable';
      }
      return 'Enable';
    }

    private updateProducts() {
      this.productService.getAll(this.currentPage, this.productsPerPage).pipe(take(1)).subscribe(products => {
        this.totalItems = Number(products.headers.get('total-count') ?? 0);
        this.products = products.body!;
      });
    }

  changePage(page: number) {
    this.currentPage = page;
    this.updateProducts();
  }

  changeSize(size: number) {
    this.productsPerPage = size;
    this.updateProducts();
  }
}
