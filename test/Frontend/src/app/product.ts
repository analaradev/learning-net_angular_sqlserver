export interface Product {
  productId: number;
  name: string;
  productNumber: string;
  listPrice: number;
  color?: string | null;
  weight?: number | null;
}
