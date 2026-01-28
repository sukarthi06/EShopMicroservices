import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.prod';
import { BasketCheckoutRequest, BasketCheckoutResponse, DeleteBasketResponse, GetBasketResponse, ShoppingCartModel, StoreBasketRequest, StoreBasketResponse } from '../../types/basket';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/basket-service/';

  async getBasket(userName: string): Promise<GetBasketResponse> {
    return firstValueFrom(
      this.http.get<GetBasketResponse>(`${this.baseUrl}basket/${userName}`)
    );
  }

  async storeBasket(cart: StoreBasketRequest): Promise<StoreBasketResponse> {
    return firstValueFrom(
      this.http.post<StoreBasketResponse>(this.baseUrl + 'basket', { cart })
    );
  }

  async deleteBasket(userName: string): Promise<DeleteBasketResponse> {
    return firstValueFrom(
      this.http.delete<DeleteBasketResponse>(this.baseUrl + `basket/${userName}`)
    );
  }

  async checkoutBasket(basketCheckoutRequest: BasketCheckoutRequest): Promise<BasketCheckoutResponse> {
    return firstValueFrom(
      this.http.post<BasketCheckoutResponse>(this.baseUrl + 'basket/checkout', 
        { BasketCheckoutDto: basketCheckoutRequest.BasketCheckoutDto })
    );
  }

  async loadUserBasket(): Promise<ShoppingCartModel> {
    const userName = "kart123"; //localStorage.getItem('userName');
    return (await this.getBasket(userName)).cart;
  }
}
