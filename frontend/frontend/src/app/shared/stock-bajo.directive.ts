import { Directive, computed, input } from '@angular/core';

// Directiva de atributo: resalta la celda cuando el stock es menor al limite
@Directive({
  selector: '[appStockBajo]',
  host: {
    '[class.stock-bajo]': 'esBajo()',
    '[attr.title]': 'esBajo() ? "Stock bajo" : null',
  },
})
export class StockBajoDirective {
  appStockBajo = input.required<number>();
  limite = input(5);
  esBajo = computed(() => this.appStockBajo() < this.limite());
}
