import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('Frontend');

  mensaje = signal('Cargando respuesta...');
  fecha = signal('');

  productos = signal<any[]>([]);

  perfil = signal<any>(null);

  busqueda = signal('');

  productoSeleccionado = signal<any>(null);

  async ngOnInit() {
    try {
      const respuesta = await fetch('/api/hello');

      if (!respuesta.ok) {
        throw new Error(`La API respondió con estado ${respuesta.status}`);
      }

      const datos = await respuesta.json();

      this.mensaje.set(datos.message);
      this.fecha.set(datos.date);

      await this.obtenerProductos();

      //perfil
      const respuestaPerfil = await fetch('/api/perfil');
      const perfil = await respuestaPerfil.json();
      this.perfil.set(perfil);

    } catch (error) {
      console.error(error);
      this.mensaje.set('No se pudo conectar con la API.');
      this.fecha.set('');
    }
  }

  async obtenerProductos() {
    try {
      const respuesta = await fetch('/api/productos');

      if (!respuesta.ok) {
        throw new Error(`La API respondió con estado ${respuesta.status}`);
      }

      const productos = await respuesta.json();
      this.productos.set(productos);
    } catch (error) {
      console.error(error);
      this.mensaje.set('Error al cargar productos.');
    }
  }

  async buscarProductos() {
    const nombre = this.busqueda().trim();

    if (!nombre) {
      await this.obtenerProductos();
      return;
    }

    try {
      const respuesta = await fetch(`/api/productos/buscar?nombre=${encodeURIComponent(nombre)}`);

      if (!respuesta.ok) {
        throw new Error(`La API respondió con estado ${respuesta.status}`);
      }

      const productos = await respuesta.json();
      this.productos.set(productos);
    } catch (error) {
      console.error(error);
      this.mensaje.set('Error al buscar productos.');
    }
  }

  async limpiarBusqueda() {
    this.busqueda.set('');
    await this.obtenerProductos();
  }

  async verDetalle(productId: number) {
    try {
      const respuesta = await fetch(`/api/productos/${productId}`);

      if (!respuesta.ok) {
        throw new Error(`La API respondió con estado ${respuesta.status}`);
      }

      const producto = await respuesta.json();
      this.productoSeleccionado.set(producto);
    } catch (error) {
      console.error(error);
      this.mensaje.set('Error al cargar el detalle del producto.');
    }
  }

  cerrarDetalle() {
    this.productoSeleccionado.set(null);
  }
    
} 
