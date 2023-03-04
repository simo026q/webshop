import {ProductVariantRequest} from "./ProductVariantRequest";
import {Category} from "../responses/Category";

export interface ProductRequest {

  id: string;

  name?: string;

  description?: string;

  imageUrl?: string;

  isActive: boolean;

  categories: Category[];

  variants: ProductVariantRequest[];
}
