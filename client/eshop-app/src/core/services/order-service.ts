import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { first, firstValueFrom } from 'rxjs';
import { GetOrdersByCustomerResponse } from '../../types/order';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + '/ordering-service/';
  
  async getOrdersByCustomer(customerId: string) {
    return await firstValueFrom(
      this.http.get<GetOrdersByCustomerResponse>(this.baseUrl + `orders/customer/${customerId}`)
    );
  } 
}
