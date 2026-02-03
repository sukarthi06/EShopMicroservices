import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { BasketCheckoutRequest, BasketCheckoutResponse, CheckBasketResponse, DeleteBasketResponse, GetBasketResponse, ShoppingCartItemModel, ShoppingCartModel, StoreBasketRequest, StoreBasketResponse } from '../../types/basket';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/basket-service/';

  basketCount = signal<number>(0);

  async getBasket(userName: string): Promise<GetBasketResponse> {
    return await firstValueFrom(
      this.http.get<GetBasketResponse>(`${this.baseUrl}basket/${userName}`)
    );
  }

  async storeBasket(request: StoreBasketRequest): Promise<StoreBasketResponse> {
    return await firstValueFrom(
      this.http.post<StoreBasketResponse>(this.baseUrl + 'basket', request)
    );
  }

  async deleteBasket(userName: string): Promise<DeleteBasketResponse> {
    return await firstValueFrom(
      this.http.delete<DeleteBasketResponse>(this.baseUrl + `basket/${userName}`)
    );
  }

  async checkoutBasket(request: BasketCheckoutRequest): Promise<BasketCheckoutResponse> {
    return await firstValueFrom(
      this.http.post<BasketCheckoutResponse>(this.baseUrl + 'basket/checkout', 
        { BasketCheckoutDto: request.BasketCheckoutDto })
    );
  }

  async checkBasket(userName: string): Promise<CheckBasketResponse> {
    return await firstValueFrom(
      this.http.get<CheckBasketResponse>(`${this.baseUrl}checkbasket/${userName}`)
    );
  }

  async loadUserBasket(): Promise<ShoppingCartModel> {
    const userName = "kart123"; //localStorage.getItem('userName');
    const cart = (await this.getBasket(userName)).cart;
    this.basketCount.set(cart.items.length);
    return cart;
  }

  async addItemToBasket(item: ShoppingCartItemModel): Promise<StoreBasketResponse> {

    const isBasketExists = (await this.checkBasket("kart123")).isSuccess; //localStorage.getItem('userName') || '');
    if(isBasketExists) {
      const basket = await this.loadUserBasket().then(cart => {
        const itemIndex = cart.items.findIndex(i => i.productId === item.productId);
        if (itemIndex !== -1) {
          cart.items[itemIndex].quantity += item.quantity;
        } else {
          cart.items.push({
            productId: item.productId,
            productName: item.productName,
            quantity: item.quantity,
            color: item.color,
            price: item.price
          });
        }
        //cart.totalPrice = cart.items.reduce((total, item) => total + item.price * item.quantity, 0);
        cart.userName = "kart123"; //localStorage.getItem('userName') || '';
        return cart;
      });
      return await this.storeBasket({ cart: basket });
    } else {
      const newBasket: ShoppingCartModel = {
        userName: "kart123", //localStorage.getItem('userName') || '',
        items: [item]
        //totalPrice: item.price * item.quantity
      };
      return await this.storeBasket({ cart: newBasket });      
    }      
  }
}
