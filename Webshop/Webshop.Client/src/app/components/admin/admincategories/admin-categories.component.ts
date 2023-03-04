import { Component } from '@angular/core';
import {RouterLink} from "@angular/router";
import {CategoryService} from "../../../services/category.service";
import {Category} from "../../../interfaces/responses/Category";
import {single} from "rxjs/operators";
import {NgForOf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {PaginationComponent} from "../../pagination/pagination.component";
import {take} from "rxjs";

@Component({
    selector: 'app-admincategories',
    templateUrl: './admin-categories.component.html',
    styleUrls: ['./admin-categories.component.scss'],
  imports: [
    RouterLink,
    NgForOf,
    FormsModule,
    PaginationComponent
  ],
    standalone: true
})
export class AdminCategoriesComponent {
  categories: Category[] = [];
  createCategoryId: string = '';

  constructor(private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.categoryService
      .getAll()
      .pipe(single())
      .subscribe(x => this.categories = x);
  }

  deleteCategory(categoryId: string) {
    if (!confirm('Are you sure you want to delete ' + categoryId + ' category?')) return;

    this.categoryService.delete(categoryId).pipe(single()).subscribe(() => {
      this.categories = this.categories.filter(x => x.id !== categoryId);
    });
  }

  createCategory() {
    const newCategory: Category = {
      id: this.createCategoryId
    }

    this.categoryService.create(newCategory).pipe(single()).subscribe(category => {
      this.categories.push(category);
    });
  }
}
