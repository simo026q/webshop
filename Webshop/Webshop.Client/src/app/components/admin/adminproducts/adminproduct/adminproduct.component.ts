import {Component, OnInit} from '@angular/core';
import {ProductService} from "../../../../services/product.service";
import {Product} from "../../../../interfaces/responses/Product";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {CurrencyPipe, NgForOf, NgIf} from "@angular/common";
import {Category} from "../../../../interfaces/responses/Category";
import {CategoryService} from "../../../../services/category.service";
import {take} from "rxjs";
import {ProductVariant} from "../../../../interfaces/responses/ProductVariant";

interface SelectCategory extends Category{
  selected?: boolean;
}

@Component({
  selector: 'app-adminproduct',
  templateUrl: './adminproduct.component.html',
  styleUrls: ['./adminproduct.component.scss'],
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    CurrencyPipe,
    RouterLink
  ],
  standalone: true
})

export class AdminproductComponent implements OnInit {

  product: Product = {
    categories: [],
    createdAt: "",
    description: "",
    fromPrice: 0,
    id: "",
    imageUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
    isActive: true,
    name: "",
    variants: []
  }
  categories?: SelectCategory[];

  newVariant: ProductVariant = {
    id: "",
    name: "",
    description: "",
    stock: 0,
    purchasePrice: 0,
    originalPrice: 0,
    sellingPrice: 0,
    isActive: true,
  }

  selectedVariant: ProductVariant = {} as ProductVariant;

  constructor(
    private productService: ProductService,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.categoryService.getAll().pipe(take(1)).subscribe(categories => {
      this.categories = categories
      this.route.paramMap.subscribe(params => {
        const action = params.get('action')!;
        if (action === 'add') {
          this.product = {
            categories: [],
            createdAt: "",
            description: "",
            fromPrice: 0,
            id: "",
            imageUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
            isActive: false,
            name: "",
            variants: []
          }
        } else {
          const id = params.get('id')!;
          this.productService.get(id).subscribe(product => {
            if (product.imageUrl === undefined || product.imageUrl === null || product.imageUrl === "") {
              product.imageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png";
            }
            this.product.categories = product.categories as SelectCategory[];
            this.product.categories.forEach(category => {
              if (this.categories) {
                this.categories.forEach(c => {
                  if (c.id === category.id) {
                    c.selected = true;
                  }
                })
              }
            })
            this.product = product;
          });
        }
      });
    })
  }

  saveProduct() {
    this.product.categories = this.categories?.filter(c => c.selected) as Category[];
    this.productService.update(this.product).subscribe(() => {
      this.router.navigate(['/admin/products']);
    })
  }

  addProduct() {
    this.product.categories = this.categories?.filter(c => c.selected) as Category[];
    this.product.id = '00000000-0000-0000-0000-000000000000';
    this.product.isActive = true;
    this.productService.create(this.product).subscribe(() => {
      this.router.navigate(['/admin/products']);
    })
  }

  deleteVariant(id: string) {
    this.product.variants = this.product.variants.filter(v => v.id !== id);
  }

  addVariant() {
    this.newVariant.id = '00000000-0000-0000-0000-000000000000';
    this.product.variants.push(this.newVariant);
    this.newVariant = {
      id: "",
      name: "",
      description: "",
      stock: 0,
      purchasePrice: 0,
      originalPrice: 0,
      sellingPrice: 0,
      isActive: true,
    }
  }

  editVariant(variant: ProductVariant) {
    this.selectedVariant = variant;
  }

  applyEditVariant() {
    this.product.variants = this.product.variants.map(v => {
      if (v.id === this.selectedVariant?.id) {
        return this.selectedVariant;
      }
      return v;
    })
    this.selectedVariant = {} as ProductVariant;
  }
}
