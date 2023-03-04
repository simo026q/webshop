import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {CurrencyPipe, NgClass, NgForOf, NgIf} from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Product } from "../../interfaces/responses/Product";
import {ProductCardComponent} from "./product-card/product-card.component";
import {ProductService} from "../../services/product.service";
import {CategoryService} from "../../services/category.service";
import {take} from "rxjs";
import {Category} from "../../interfaces/responses/Category";
import {PaginationComponent} from "../pagination/pagination.component";
import {single} from "rxjs/operators";
import {error} from "@angular/compiler-cli/src/transformers/util";

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
  standalone: true,
  imports: [
    NgForOf,
    FormsModule,
    CurrencyPipe,
    RouterLink,
    NgClass,
    ProductCardComponent,
    NgIf,
    PaginationComponent,
  ]
})

export class ProductsComponent implements OnInit {
  searchFilter?: string;
  categoryFilter: string = "All";

  categories: Category[] = [];
  selectedCategory: string = "All";
  products: Product[] = [];

  totalItems = 0;
  currentPage = 1;
  productsPerPage = 12;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private categoryService: CategoryService,
  ) {}

  ngOnInit(): void {
    this.categoryService.getAllWithProducts().pipe(single()).subscribe(categories => {
      this.categories = categories;

      this.route.queryParams.subscribe(params => {
        const category = params['category'];
        this.searchFilter = params['search'];

        const isValid = this.categories.find(x => x.id == category) != undefined;

        if (!isValid) {
          this.selectedCategory = 'All';
          this.navigateTo(this.selectedCategory, this.searchFilter);
        }
        else {
          this.selectedCategory = category;
        }

        this.categoryFilter = this.selectedCategory;

        this.getProductsByCategory(this.currentPage, this.productsPerPage, this.selectedCategory, this.searchFilter);
      });
    });
  }

  private navigateTo(category: string, search?: string) {
    this.router.navigate(['/products'], { queryParams: { category, search } });
  }

  canApplyFilter(): boolean {
    const search = this.route.snapshot.queryParams['search'];
    const searchFilter = (this.searchFilter && this.searchFilter != '') ? this.searchFilter : undefined;

    return this.categoryFilter != this.selectedCategory || searchFilter != search;
  }

  applyFilter() {
    const search = (this.searchFilter && this.searchFilter != '') ? this.searchFilter : undefined;

    this.navigateTo(this.categoryFilter, search);
  }

  getProductsByCategory(page: number, productsPerPage: number, category: string, search?: string): void {
    const categoryParam = (category == 'All' || category == '') ? undefined : category;

    this.productService.getAll(page, productsPerPage, search, categoryParam)
      .pipe(take(1))
      .subscribe({
        next: (productsResponse) => {
          this.totalItems = Number(productsResponse.headers.get('total-count'));
          this.products = productsResponse.body!.filter(x => x.isActive);
        },
        error: (err) => {
          console.error(err);
          this.products = [];
        },
      });
  }

  changePage(page: number) {
    this.currentPage = page;
    this.getProductsByCategory(this.currentPage, this.productsPerPage, this.selectedCategory);
  }

  changeSize(size: number) {
    this.productsPerPage = size;
    this.getProductsByCategory(this.currentPage, this.productsPerPage, this.selectedCategory);
  }
}
