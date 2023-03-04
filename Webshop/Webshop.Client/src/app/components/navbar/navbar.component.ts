import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from "../../core/services/authentication.service";
import {Router} from "@angular/router";
import {CategoryService} from "../../services/category.service";
import {Category} from '../../interfaces/responses/Category';
import { NgForOf } from '@angular/common';
import { NavbarUserComponent } from '../navbar-user/navbar-user.component';
import {single} from "rxjs/operators";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styles: []
})
export class NavbarComponent implements OnInit {
  categories: Category[] = [];

  constructor(
    public authService: AuthenticationService,
    private router: Router,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.categoryService
      .getAllWithProducts()
      .pipe(single())
      .subscribe(x => this.categories = x);
  }
}
