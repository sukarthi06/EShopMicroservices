import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { Product } from '../../../types/product';
import { Router, RouterLink } from '@angular/router';
import { BasketService } from '../../../core/services/basket-service';
import { ImageService } from '../../../core/services/image-service';

@Component({
  selector: 'app-product-card',
  imports: [RouterLink],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard implements OnInit {
  
  @Input({ required: true }) product!: Product;
  
  protected basketService = inject(BasketService);
  protected imageService = inject(ImageService);
  protected imageUrl = signal<string>('');
  protected isLoading = false;

  private router = inject(Router);

  async ngOnInit(): Promise<void> {
    this.imageUrl.set('assets/images/product/' + this.product.imageFile);    
  }

  async addToCart(): Promise<void> {

    this.isLoading = true;
    await this.basketService.addItemToBasket({
      productId: this.product.id,
      productName: this.product.name,
      quantity: 1,
      color: 'Black',
      price: this.product.price
    }).then(() => {
      this.isLoading = false;
      this.router.navigate(['/cart']);
    });

  }
}
