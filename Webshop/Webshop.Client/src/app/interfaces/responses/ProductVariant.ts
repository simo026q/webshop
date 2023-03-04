export interface ProductVariant {

  id: string;

  name?: string;

  description?: string;

  stock: number;

  purchasePrice?: number;

  originalPrice: number;

  sellingPrice: number;

  isActive: boolean;
}
