import {ProductVariant} from "./ProductVariant";
import {Category} from "./Category";

export interface Product {

  id: string;

  name?: string;

  description?: string;

  imageUrl?: string;

  isActive: boolean;

  fromPrice: number;

  createdAt: string;

  categories: Category[];

  variants: ProductVariant[];
}
