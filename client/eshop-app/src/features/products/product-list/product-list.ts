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
  private allProducts = signal<Product[]>([]);
  protected productService = inject(ProductService);
  protected categories: string[] = [];

  protected readonly lastProduct = computed(() => {
    const items = this.products();
    return items.length ? items[items.length - 1] : undefined;
  });

  ngOnInit(): void {
    this.productService.getProducts().subscribe({
      next: response => {        
        this.products.set(response.products);
        this.allProducts.set(response.products);
        this.setCategories();
      },
      error: error => console.log(error)
    });
  }

  setCategories(): void {
    const uniqueCategories = this.products()
      .flatMap(p => p.category)
      .filter((value, index, self) => self.indexOf(value) === index);

    this.categories = ['All', ...uniqueCategories];
  }

  filterProductsByCategory(category: string): void {
    this.products.set(this.allProducts());
    if (category === 'All') return;
    const filteredProducts = this.products().filter(p => p.category.includes(category));
    this.products.set(filteredProducts);
  }

}
