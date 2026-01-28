import { Component, Input } from '@angular/core';
import { Product } from '../../../types/product';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-product-card',
  imports: [RouterLink],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard {
  @Input({ required: true }) product!: Product;

  addToCart(): void {
    //this.cartService.addToCart(this.product.id);
  }
}
