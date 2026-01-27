import { Component, inject, OnInit, signal } from '@angular/core';
import { Product } from '../../../types/product';
import { ProductService } from '../../../core/services/product-service';
import { ActivatedRoute, RouterLink } from "@angular/router";

@Component({
  selector: 'app-product-detail',
  imports: [RouterLink],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.css',
})
export class ProductDetail implements OnInit {
  
  private route = inject(ActivatedRoute);  
  private productId: string = '';
  private productService = inject(ProductService);

  protected product = signal<Product | null>(null);
  protected quantity = signal<number>(1);

  ngOnInit(): void {
    
    this.productId = this.route.snapshot.paramMap.get('id') || '';    
    this.productService.getProductById(this.productId).subscribe({
      next: (response) => this.product.set(response.product),
      error: (error) => console.error('Error fetching product:', error)
    });
  }

  increaseQuantity(): void {
    this.quantity.set(this.quantity() + 1);
  }
  decreaseQuantity(): void {
    if (this.quantity() > 1) {
      this.quantity.set(this.quantity() - 1);
    } 
  }
}
