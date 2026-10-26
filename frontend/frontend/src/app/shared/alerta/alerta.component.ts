import { Component, input } from '@angular/core';

// Componente reutilizable: muestra un mensaje de error si existe
@Component({
  selector: 'app-alerta',
  template: `@if (mensaje()) { <div class="error">{{ mensaje() }}</div> }`,
})
export class AlertaComponent {
  mensaje = input('');
}
