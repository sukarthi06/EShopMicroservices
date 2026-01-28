export type ShoppingCartItemModel = {
  quantity: number;
  color: string;
  price: number;
  productId: string;
  productName: string;
}

export type ShoppingCartModel = {
  userName: string;
  items: ShoppingCartItemModel[];
  totalPrice: number;
}

export interface GetBasketResponse {
  cart: ShoppingCartModel;
}

export interface StoreBasketRequest {
  cart: ShoppingCartModel;
}

export interface StoreBasketResponse {
  userName: string;
}

export interface DeleteBasketResponse {
  isSuccess: boolean;
}

export type BasketCheckoutModel = {
  // User
  userName: string;
  customerId: string; // Guid in C# → string in TS
  totalPrice: number;

  // Shipping / Billing Address
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;

  // Payment
  cardName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentMethod: number;
};

export interface BasketCheckoutRequest {
  BasketCheckoutDto: BasketCheckoutModel;
}

export interface BasketCheckoutResponse {
  isSuccess: boolean;
}