import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ProductResponse, ProductsResponse } from '../../types/product';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductService {

  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/catalog-service/';

  async getProducts(){
    return await firstValueFrom(
      this.http.get<ProductsResponse>(this.baseUrl + 'products')
    );
  }

  async getProductById(id: string){
    return await firstValueFrom(
      this.http.get<ProductResponse>(this.baseUrl + 'products/' + id)
    );
  }
}
