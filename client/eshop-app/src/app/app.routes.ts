import { Routes } from '@angular/router';
import { ProductList } from '../features/products/product-list/product-list';
import { Home } from '../features/home/home';

export const routes: Routes = [
    { path: '', component: Home },
    { path: 'products', component: ProductList }
];
