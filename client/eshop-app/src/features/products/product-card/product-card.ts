import { Component, inject, Input } from '@angular/core';
import { Product } from '../../../types/product';
import { RouterLink } from '@angular/router';
import { BasketService } from '../../../core/services/basket-service';

@Component({
  selector: 'app-product-card',
  imports: [RouterLink],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard {
  @Input({ required: true }) product!: Product;
  protected basketService = inject(BasketService);

  async addToCart(): Promise<void> {

    await this.basketService.addItemToBasket({
      productId: this.product.id,
      productName: this.product.name,
      quantity: 1,
      color: 'Black',
      price: this.product.price
    });

  }
}
