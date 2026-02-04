import { Component, inject, OnInit, signal } from '@angular/core';
import { BasketService } from '../../../core/services/basket-service';
import { BasketCheckoutModel, ShoppingCartModel } from '../../../types/basket';
import { RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-checkout',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout implements OnInit {

  private basketService = inject(BasketService);

  protected checkoutForm: FormGroup;
  protected shoppingCart = signal<ShoppingCartModel | null>(null);
  protected totalPrice = signal<number>(0);
  protected basketCheckout = signal<BasketCheckoutModel | null>(null);
  
  // List of US states
  protected states: string[] = [
    'Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 
    'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 
    'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana', 
    'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi',
    'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire', 'New Jersey',
    'New Mexico', 'New York', 'North Carolina', 'North Dakota', 'Ohio', 'Oklahoma',
    'Oregon', 'Pennsylvania', 'Rhode Island', 'South Carolina', 'South Dakota',
    'Tennessee', 'Texas', 'Utah', 'Vermont', 'Virginia', 'Washington', 
    'West Virginia', 'Wisconsin', 'Wyoming'
  ];

  constructor() {
    this.checkoutForm = new FormGroup({
      firstName: new FormControl('',Validators.required),
      lastName: new FormControl('',Validators.required),
      email: new FormControl('',[Validators.email, Validators.required]),
      addressLine: new FormControl('',Validators.required),
      country: new FormControl('',Validators.required),
      state: new FormControl('',Validators.required),
      zipCode: new FormControl('',Validators.required),
      cardName: new FormControl('',Validators.required),
      cardNumber: new FormControl('',Validators.required),
      expiration: new FormControl('',Validators.required),
      cvv: new FormControl('',Validators.required)
    });
  }

  async ngOnInit(): Promise<void> {
    
    await this.basketService.loadUserBasket().then(cart => {
      this.shoppingCart.set(cart);
      this.totalPrice.set(cart.items.reduce((total, item) => total + (item.price * item.quantity), 0));
    });
  }

  async checkoutBasket() {
    
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }
    const cart = this.shoppingCart();    
    if (!cart || cart.items.length === 0) return;

    const formValue = this.checkoutForm.getRawValue();
    const checkoutModel: BasketCheckoutModel = {
      userName: cart.userName,
      customerId: '58c49479-ec65-4de2-86e7-033c546291aa', // Placeholder GUID
      totalPrice: this.totalPrice(),
      firstName: formValue.firstName,
      lastName: formValue.lastName,
      emailAddress: formValue.email,
      addressLine: formValue.addressLine,
      country: formValue.country,
      state: formValue.state,
      zipCode: formValue.zipCode,
      cardName: formValue.cardName,
      cardNumber: formValue.cardNumber,
      expiration: formValue.expiration,
      cvv: formValue.cvv,
      paymentMethod: 1 // Assuming 1 represents a specific payment method
    };
    
    // await this.basketService.checkoutBasket({ BasketCheckoutDto: checkoutModel }).then(response => {
    //   if (response.isSuccess) {
    //     // Handle successful checkout (e.g., navigate to confirmation page)
    //     console.log('Checkout successful');
    //   } else {
    //     // Handle checkout failure (e.g., show error message)
    //     console.log('Checkout failed');
    //   }
    // });
        
  }

}