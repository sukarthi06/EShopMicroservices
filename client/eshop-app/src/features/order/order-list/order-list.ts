import { Component, inject, OnInit, signal } from '@angular/core';
import { OrderService } from '../../../core/services/order-service';
import { OrderModel } from '../../../types/order';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-order-list',
  imports: [RouterLink],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css',
})
export class OrderList implements OnInit {

  private orderService = inject(OrderService);
  
  protected orders = signal<OrderModel[] | null>(null);
  protected totalPrice = signal<number>(0);

  async ngOnInit() {
    const customerId = "58c49479-ec65-4de2-86e7-033c546291aa"; //localStorage.getItem('customerId') || '';
    const response = await this.orderService.getOrdersByCustomer(customerId);
    this.orders.set(response.orders);
    this.totalPrice.set(response.orders.reduce((total, order) => {
      const orderTotal = order.orderItems.reduce((orderSum, item) => orderSum + (item.price * item.quantity), 0);
      return total + orderTotal;
    }, 0));
    
  }

}
