import {Address} from "../Address";
import {OrderProductRequest} from "./OrderProductRequest";

export interface OrderRequest {
  id: string;
  userId: string;
  address: Address;
  products: OrderProductRequest[];
}
