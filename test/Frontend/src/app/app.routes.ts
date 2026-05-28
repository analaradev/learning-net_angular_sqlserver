import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Products } from './pages/products/products';
import { ProductDetail } from './pages/product-detail/product-detail';

export const routes: Routes = [
  {
    path: '',
    component: Home
  },
  {
    path: 'productos',
    component: Products
  },
  {
    path: 'productos/:id',
    component: ProductDetail
  }
];