export interface Categoria {
  id: number;
  nombre: string;
  descripcion: string | null;
  cantidadProductos: number;
}

export interface CategoriaRequest {
  nombre: string;
  descripcion: string;
}
