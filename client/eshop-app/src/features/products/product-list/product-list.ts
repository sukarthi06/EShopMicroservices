import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ProductService } from '../../../core/services/product-service';
import { Product } from '../../../types/product';

@Component({
  selector: 'app-product-list',
  imports: [],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
    
  protected products = signal<Product[]>([]);
  protected readonly lastProduct = computed(() => {
  const items = this.products();
  return items.length ? items[items.length - 1] : undefined;
});
  protected productService = inject(ProductService);

  ngOnInit(): void {
    this.productService.getProducts().subscribe({
      next: response => {
  console.log('API response:', response);
  console.log('Is array?', Array.isArray(response));
  this.products.set(response.products);
},
      error: error => console.log(error)
    });
  }

}
