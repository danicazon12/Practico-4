import { Pipe, PipeTransform } from '@angular/core';

// Pipe: 12.5 -> "Bs 12.50"
@Pipe({ name: 'precioBs' })
export class PrecioBsPipe implements PipeTransform {
  transform(valor: number): string {
    return 'Bs ' + valor.toFixed(2);
  }
}
