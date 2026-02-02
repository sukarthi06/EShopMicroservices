import { Component, inject, OnInit, signal } from '@angular/core';
import { Product } from '../../../types/product';
import { ProductService } from '../../../core/services/product-service';
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { BasketService } from '../../../core/services/basket-service';

@Component({
  selector: 'app-product-detail',
  imports: [RouterLink],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.css',
})
export class ProductDetail implements OnInit {
  
  private route = inject(ActivatedRoute);  
  private router = inject(Router);
  private productId: string = '';
  private productService = inject(ProductService);
  private basketService = inject(BasketService);

  protected product = signal<Product | null>(null);
  protected quantity = signal<number>(1);
  protected selectedColor = signal<string>('Black');
  
  async ngOnInit(): Promise<void> {

    try {
      this.productId = this.route.snapshot.paramMap.get('id') || '';
      const response = await this.productService.getProductById(this.productId);
      this.product.set(response.product);
    } catch (error) {
      console.error(error);
    }
  }

  increaseQuantity(): void {
    this.quantity.set(this.quantity() + 1);
  }
  decreaseQuantity(): void {
    if (this.quantity() > 1) {
      this.quantity.set(this.quantity() - 1);
    } 
  }

  async addToCart(): Promise<void> {
    if (!this.product()) return;

    await this.basketService.addItemToBasket({
      productId: this.product()!.id,
      productName: this.product()!.name,
      quantity: this.quantity(),
      color: this.selectedColor(),
      price: this.product()!.price
    }).then(() => {
      this.router.navigate(['/cart']);
    });
  }
}
