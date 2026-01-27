export type Product = {
    id: string;
    name: string;
    category: string[];
    description: string;
    imageFile: string;
    price: number;
}

export interface ProductsResponse {
  products: Product[];
}

export interface ProductResponse {
  product: Product;
}