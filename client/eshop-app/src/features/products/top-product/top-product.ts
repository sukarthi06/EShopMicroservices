import { Component, Input } from '@angular/core';
import { Product } from '../../../types/product';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-top-product',
  imports: [RouterLink],
  templateUrl: './top-product.html',
  styleUrl: './top-product.css',
})
export class TopProduct {
  @Input({ required: true }) product!: Product;
}
