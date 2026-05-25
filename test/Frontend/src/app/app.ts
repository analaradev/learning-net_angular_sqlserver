import { Component, signal } from '@angular/core';

import { API_ROUTES } from './api-routes';
import { Product } from './product';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  products = signal<Product[]>([]);
  selectedProduct = signal<Product | null>(null);
  searchText = signal('');
  error = signal('');

  async ngOnInit() {
    await this.loadProducts();
  }

  async loadProducts() {
    try {
      const response = await fetch(API_ROUTES.products);

      if (!response.ok) {
        throw new Error('No se pudo cargar la lista de productos.');
      }

      const products = await response.json();
      this.products.set(products);
      this.error.set('');
    } catch {
      this.error.set('No se pudieron cargar los productos.');
    }
  }

  async searchProducts() {
    const name = this.searchText().trim();

    if (!name) {
      await this.loadProducts();
      return;
    }

    try {
      const response = await fetch(API_ROUTES.searchProducts(name));

      if (!response.ok) {
        throw new Error('No se pudo buscar productos.');
      }

      const products = await response.json();
      this.products.set(products);
      this.selectedProduct.set(null);
      this.error.set('');
    } catch {
      this.error.set('No se pudo buscar productos.');
    }
  }

  async viewDetail(productId: number) {
    try {
      const response = await fetch(API_ROUTES.productDetail(productId));

      if (!response.ok) {
        throw new Error('No se pudo cargar el detalle del producto.');
      }

      const product = await response.json();
      this.selectedProduct.set(product);
      this.error.set('');
    } catch {
      this.error.set('No se pudo cargar el detalle del producto.');
    }
  }
}
