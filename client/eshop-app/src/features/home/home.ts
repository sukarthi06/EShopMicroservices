import { Component, inject, OnInit, signal } from '@angular/core';
import { ProductService } from '../../core/services/product-service';
import { Product } from '../../types/product';
import { TopProduct } from "../products/top-product/top-product";
import { ProductCard } from "../products/product-card/product-card";

@Component({
  selector: 'app-home',
  imports: [TopProduct, ProductCard],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  
  protected product: Product | null = null;
  protected products = signal<Product[]>([]);
  protected productService = inject(ProductService);

  protected readonly topProduct = () => {
    const items = this.products();
    return items.length ? items[0] : undefined;
  };

  protected readonly bestProducts = () => {
    const items = this.products();
    return items.slice(0, 4);
  };

  protected readonly latestProducts = () => {
    const items = this.products();
    return items.slice(-4);
  }

  async ngOnInit() {
    try {
      const response = await this.productService.getProducts();
      this.products.set(response.products);
    } catch (error) {
      console.error(error);
    }
  }
}