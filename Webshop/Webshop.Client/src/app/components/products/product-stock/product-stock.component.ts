import {Component, Input} from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-stock',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-stock.component.html',
  styles: []
})
export class ProductStockComponent {
  @Input() stock: number = 0;

  getDisplayStock(): string {
    if (this.stock === 0) {
      return 'Out of stock';
    }
    if (this.stock <= 5) {
      return '1-5 in stock';
    }
    if (this.stock <= 10) {
      return '5-10 in stock';
    }
    return '10+ in stock';
  }

  getStockColor(): string {
    if (this.stock <= 5) {
      return 'text-danger';
    } else if (this.stock <= 10) {
      return 'text-warning';
    } else {
      return 'text-success';
    }
  }
}
