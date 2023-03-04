import { Product } from "./responses/Product";
import { ProductVariant } from "./responses/ProductVariant";

export interface BasketProduct {
  product: Product,
  productVariant: ProductVariant,
  quantity: number
}