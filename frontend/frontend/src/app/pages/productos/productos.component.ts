import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Categoria } from '../../models/categoria';
import { Producto, ProductoRequest } from '../../models/producto';
import { CategoriaService } from '../../services/categoria.service';
import { ProductoService } from '../../services/producto.service';
import { mensajeDeError } from '../../shared/mensaje-error';
import { AlertaComponent } from '../../shared/alerta/alerta.component';
import { EstadoPipe } from '../../shared/estado.pipe';
import { PrecioBsPipe } from '../../shared/precio-bs.pipe';
import { StockBajoDirective } from '../../shared/stock-bajo.directive';

@Component({
  selector: 'app-productos',
  imports: [FormsModule, AlertaComponent, EstadoPipe, PrecioBsPipe, StockBajoDirective],
  templateUrl: './productos.component.html',
})
export class ProductosComponent implements OnInit {
  private productoService = inject(ProductoService);
  private categoriaService = inject(CategoriaService);

  productos = signal<Producto[]>([]);
  categorias = signal<Categoria[]>([]);
  error = signal('');
  editandoId = signal<number | null>(null);
  form: ProductoRequest = this.formVacio();

  ngOnInit() {
    this.categoriaService.obtenerTodas().subscribe({
      next: (datos) => this.categorias.set(datos),
      error: (e) => this.error.set(mensajeDeError(e)),
    });
    this.cargar();
  }

  formVacio(): ProductoRequest {
    return { nombre: '', precio: 0, stock: 0, estado: true, categoriaId: 0 };
  }

  cargar() {
    this.productoService.obtenerTodos().subscribe({
      next: (datos) => this.productos.set(datos),
      error: (e) => this.error.set(mensajeDeError(e)),
    });
  }

  guardar() {
    this.error.set('');
    const id = this.editandoId();
    const peticion = id === null
      ? this.productoService.crear(this.form)
      : this.productoService.actualizar(id, this.form);

    peticion.subscribe({
      next: () => {
        this.cancelar();
        this.cargar();
      },
      error: (e) => this.error.set(mensajeDeError(e)),
    });
  }

  editar(producto: Producto) {
    this.editandoId.set(producto.id);
    this.form = {
      nombre: producto.nombre,
      precio: producto.precio,
      stock: producto.stock,
      estado: producto.estado,
      categoriaId: producto.categoriaId,
    };
  }

  cancelar() {
    this.editandoId.set(null);
    this.form = this.formVacio();
  }

  eliminar(producto: Producto) {
    if (!confirm('¿Eliminar el producto ' + producto.nombre + '?')) {
      return;
    }
    this.error.set('');
    this.productoService.eliminar(producto.id).subscribe({
      next: () => this.cargar(),
      error: (e) => this.error.set(mensajeDeError(e)),
    });
  }
}
