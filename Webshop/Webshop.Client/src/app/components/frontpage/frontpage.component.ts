import {Component, OnInit} from '@angular/core';
import {RouterLink} from "@angular/router";
import {Product} from "../../interfaces/responses/Product";
import {ProductService} from "../../services/product.service";
import {NgForOf, NgIf} from "@angular/common";
import {ProductCardComponent} from "../products/product-card/product-card.component";

@Component({
    selector: 'app-frontpage',
    templateUrl: './frontpage.component.html',
    styleUrls: ['./frontpage.component.scss'],
    imports: [
        RouterLink,
        NgForOf,
        ProductCardComponent,
        NgIf
    ],
    standalone: true
})
export class FrontpageComponent implements OnInit {
    products: Product[] = [];

    constructor(private productService: ProductService) {}

    ngOnInit(): void {
        this.productService.getLatest().subscribe(products => {
            this.products = products;
        });
    }
}
