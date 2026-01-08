import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Product, ProductsResponse } from '../../types/product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {

  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/catalog-service/';

  getProducts(){
    return this.http.get<ProductsResponse>(this.baseUrl + 'products');
  }
}
