export const API_ROUTES = {
  products: '/api/productos',
  productDetail: (id: number) => `/api/productos/${id}`,
  searchProducts: (name: string) => `/api/productos/buscar?nombre=${encodeURIComponent(name)}`
};
