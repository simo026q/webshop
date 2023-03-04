import { Component } from '@angular/core';
import {RouterLink} from "@angular/router";
import {DatePipe, NgForOf} from "@angular/common";
import {take} from "rxjs";
import {User} from "../../../interfaces/responses/User";
import {UserService} from "../../../services/user.service";
import {PaginationComponent} from "../../pagination/pagination.component";
import {CurrentAccountService} from "../../../services/currentAccount.service";

@Component({
    selector: 'app-adminusers',
    templateUrl: './adminusers.component.html',
    styleUrls: ['./adminusers.component.scss'],
  imports: [
    RouterLink,
    NgForOf,
    PaginationComponent,
    DatePipe
  ],
    standalone: true
})
export class AdminusersComponent {

  users: User[] = [];
  currentPage = 1;
  productsPerPage = 12;
  totalItems = 0;
  constructor(
    private userService: UserService,
    private currentAccountService: CurrentAccountService,

  ) { }

  ngOnInit(): void {
    this.getUsers();
  }

  private getUsers() {
    this.userService.getAll(this.currentPage, this.productsPerPage).pipe(take(1)).subscribe(users => {
      this.totalItems = Number(users.headers.get('total-count'));
      this.users = users.body!;
    });
  }

  changePage(page: number) {
    this.currentPage = page;
    this.getUsers();
  }

  changeSize(size: number) {
    this.productsPerPage = size;
    this.getUsers();
  }

  deleteUser(user: User) {
    let currentUser;
    this.currentAccountService.currentUser?.pipe(take(1)).subscribe(account => {
      currentUser = account;
      if (confirm('Are you sure you want to delete this user?')) {
        if (user.role === 2) {
          alert('You cannot delete an admin user');
          return;
        }
        if (user.id === currentUser.id) {
          alert('You cannot delete yourself');
          return;
        }
        this.userService.delete(user.id).pipe(take(1)).subscribe();
        this.users = this.users.filter(x => x.id !== user.id);
      }
    });
  }
}
