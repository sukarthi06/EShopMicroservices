import { Routes } from '@angular/router';
import { ProductList } from '../features/products/product-list/product-list';
import { Home } from '../features/home/home';
import { ProductDetail } from '../features/products/product-detail/product-detail';
import { Cart } from '../features/cart/cart';

export const routes: Routes = [
    { path: '', component: Home },
    { path: 'products', component: ProductList },
    { path: 'productdetail/:id', component: ProductDetail },
    { path: 'cart', component: Cart }
];
