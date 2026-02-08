import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterOutlet } from "@angular/router";
import { Nav } from "../layout/nav/nav";
import { Footer } from "../layout/footer/footer";
import { BasketService } from '../core/services/basket-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Nav, Footer, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('eshop-app');
  protected basketService = inject(BasketService);

  /**
   *
   */
  async ngOnInit(): Promise<void> {
    await this.loadUserBasket();
  }

  async loadUserBasket() {

    const isBasketExists = (await this.basketService.checkBasket("kart123")).isSuccess; //localStorage.getItem('userName') || '');
    if(isBasketExists) {
      await this.basketService.loadUserBasket();
    } else {
      this.basketService.basketCount.set(0);
    }

  }
  
}
