import { PaginatedResult } from './utility';

export type OrderItemModel = {
  orderId: string;
  productId: string;
  quantity: number;
  price: number;
};

export type AddressModel = {
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;
};

export type PaymentModel = {
  cardName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentMethod: number;
};

export enum OrderStatus {
    Draft = 1,
    Pending = 2,
    Completed = 3,
    Cancelled = 4
}

export type OrderModel = {
  id: string;
  customerId: string;
  orderName: string;
  shippingAddress: AddressModel;
  billingAddress: AddressModel; 
  payment: PaymentModel;
  status: OrderStatus;
  orderItems: OrderItemModel[];
}

export interface GetOrdersResponse {
  orders: PaginatedResult<OrderModel>;
}

export interface GetOrdersByNameResponse {
  orders: OrderModel[];
}

export interface GetOrdersByCustomerResponse {
  orders: OrderModel[];
}