import {Component, EventEmitter, Input, OnInit, OnChanges, Output, SimpleChanges} from '@angular/core';
import {NgClass, NgForOf} from "@angular/common";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss'],
  standalone: true,
  imports: [
    NgClass,
    NgForOf,
    FormsModule
  ]
})
export class PaginationComponent implements OnInit, OnChanges {
  @Output() pageChange = new EventEmitter<number>();
  @Output() sizeChange = new EventEmitter<number>();

  @Input() totalItems!: number;

  currentPage = 1;
  pages: number[] = [];
  itemsPerPage = 12;

  constructor() { }

  ngOnInit(): void {
    this.setPages();
  }

  ngOnChanges(changes: SimpleChanges) {
    const totalItems = changes['totalItems'];

    if (totalItems.previousValue !== totalItems.currentValue) {
      this.setPages();
    }
  }

  private setPages() {
    // Get the pages as an array of numbers (e.g. [1, 2, 3, 4, 5])
    this.pages = [...Array(this.totalPages()).keys()].map(i => i + 1);
  }

  hasPreviousPage() {
    return this.currentPage > 1;
  }

  hasNextPage() {
    console.log('hasNextPage', this.currentPage);
    return this.currentPage < this.totalPages();
  }

  itemsOnCurrentPage() {
    return Math.min(this.currentPage * this.itemsPerPage, this.totalItems);
  }

  changePage(page: number) {
    console.log(this.currentPage);

    const actualPage = Math.min(Math.max(page, 1), this.totalPages());

    console.log(actualPage);

    if (actualPage === this.currentPage)
      return;

    this.currentPage = Math.min(Math.max(page, 1), this.totalPages());

    this.pageChange.emit(this.currentPage);
  }

  changeSize() {
    this.setPages();

    // Check if the current page is still valid
    if (this.currentPage > this.totalPages()) {
      this.changePage(this.totalPages());
    }

    this.sizeChange.emit(this.itemsPerPage);
  }

  totalPages() {
    return Math.ceil(this.totalItems / this.itemsPerPage);
  }
}
