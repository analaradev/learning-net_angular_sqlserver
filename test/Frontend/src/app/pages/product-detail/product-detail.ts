import { Component, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { API_ROUTES } from '../../api-routes';
import { Product } from '../../product';

@Component({
  selector: 'app-product-detail',
  imports: [RouterLink],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss'
})
export class ProductDetail {
  product = signal<Product | null>(null);
  error = signal('');

  constructor(private route: ActivatedRoute) {
  }

  async ngOnInit() {
    const productId = Number(this.route.snapshot.paramMap.get('id'));

    try {
      const response = await fetch(API_ROUTES.productDetail(productId));

      if (!response.ok) {
        throw new Error('No se pudo cargar el producto.');
      }

      const product = await response.json();
      this.product.set(product);
    } catch {
      this.error.set('No se pudo cargar el producto.');
    }
  }
}