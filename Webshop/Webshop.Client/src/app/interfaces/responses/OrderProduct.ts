import {ProductVariant} from "./ProductVariant";

export interface OrderProduct {
  orderId: string;
  productVariantId: string;
  totalPrice: number;
  quantity: number;
  productVariant: ProductVariant;
}
