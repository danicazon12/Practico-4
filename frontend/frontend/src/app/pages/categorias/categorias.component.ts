import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Categoria, CategoriaRequest } from '../../models/categoria';
import { CategoriaService } from '../../services/categoria.service';
import { mensajeDeError } from '../../shared/mensaje-error';
import { AlertaComponent } from '../../shared/alerta/alerta.component';

@Component({
  selector: 'app-categorias',
  imports: [FormsModule, AlertaComponent],
  templateUrl: './categorias.component.html',
})
export class CategoriasComponent implements OnInit {
  private servicio = inject(CategoriaService);

  categorias = signal<Categoria[]>([]);
  error = signal('');
  editandoId = signal<number | null>(null);
  form: CategoriaRequest = { nombre: '', descripcion: '' };

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.servicio.obtenerTodas().subscribe({
      next: (datos) => this.categorias.set(datos),
      error: (e) => this.error.set(mensajeDeError(e)),
    });
  }

  guardar() {
    this.error.set('');
    const id = this.editandoId();
    const peticion = id === null
      ? this.servicio.crear(this.form)
      : this.servicio.actualizar(id, this.form);

    peticion.subscribe({
      next: () => {
        this.cancelar();
        this.cargar();
      },
      error: (e) => this.error.set(mensajeDeError(e)),
    });
  }

  editar(categoria: Categoria) {
    this.editandoId.set(categoria.id);
    this.form = { nombre: categoria.nombre, descripcion: categoria.descripcion ?? '' };
  }

  cancelar() {
    this.editandoId.set(null);
    this.form = { nombre: '', descripcion: '' };
  }

  eliminar(categoria: Categoria) {
    if (!confirm('¿Eliminar la categoría ' + categoria.nombre + '?')) {
      return;
    }
    this.error.set('');
    this.servicio.eliminar(categoria.id).subscribe({
      next: () => this.cargar(),
      error: (e) => this.error.set(mensajeDeError(e)),
    });
  }
}
