import { HttpErrorResponse } from '@angular/common/http';

// Convierte la respuesta de error del API en un texto para mostrar
export function mensajeDeError(err: unknown): string {
  if (!(err instanceof HttpErrorResponse)) {
    return 'Ocurrió un error inesperado';
  }
  if (err.status === 0) {
    return 'No se pudo conectar con el servidor';
  }
  const cuerpo = err.error;
  if (cuerpo?.errors) {
    return (Object.values(cuerpo.errors) as string[][]).flat().join(' · ');
  }
  if (cuerpo?.mensaje) {
    return cuerpo.mensaje;
  }
  return 'Error ' + err.status;
}
