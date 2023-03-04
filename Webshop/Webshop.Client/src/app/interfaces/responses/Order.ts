import {Address} from "../Address";
import {OrderProduct} from "./OrderProduct";

export enum OrderStatus {
  Open,
  Confirmed,
  Processing,
  Completed,
  Cancelled,
}

export interface Order {
  id: string;
  addressId: string;
  userId: string;
  status: OrderStatus;
  address: Address;
  products: OrderProduct[];
  totalPrice: number;
  createdAt: Date;
}
