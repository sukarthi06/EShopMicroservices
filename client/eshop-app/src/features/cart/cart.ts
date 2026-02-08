import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from "@angular/router";
import { ShoppingCartModel } from '../../types/basket';
import { BasketService } from '../../core/services/basket-service';

@Component({
  selector: 'app-cart',
  imports: [RouterLink],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart implements OnInit {

  private basketService = inject(BasketService);

  protected cart = signal<ShoppingCartModel | null>(null!);
  protected totalPrice = signal<number>(0);
  
  async ngOnInit(): Promise<void> {
    await this.processBasket();
  }

  async processBasket(): Promise<void> {

    const isBasketExists = (await this.basketService.checkBasket("kart123")).isSuccess; //localStorage.getItem('userName') || '');
    if(!isBasketExists) {
      this.cart.set(null);
      this.totalPrice.set(0);
      return;
    }
    await this.basketService.loadUserBasket().then(cart => {
      this.cart.set(cart);
      this.totalPrice.set(cart.items.reduce((total, item) => total + (item.price * item.quantity), 0));
    });
  }

  async removeItemFromCart(productId: string): Promise<void> {
    const cart = this.cart();
    if (!cart) return;

    const itemIndex = cart.items.findIndex(i => i.productId === productId);
    if (itemIndex === -1) return;

    const updatedItems = [...cart.items];
    updatedItems.splice(itemIndex, 1);
    
    await this.basketService.storeBasket({ cart: { userName: cart.userName, items: updatedItems } }).then(() => {
      // Update local cart state after successful removal
      this.cart.set({ userName: cart.userName, items: updatedItems });
      this.totalPrice.set(updatedItems.reduce((total, item) => total + (item.price * item.quantity), 0));
      this.basketService.basketCount.set(updatedItems.length);
    });
  }

}
