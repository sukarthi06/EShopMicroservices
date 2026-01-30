import { ErrorHandler, Injectable, NgZone } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GlobalErrorHandler implements ErrorHandler {

  constructor(private zone: NgZone) {}

  handleError(error: any): void {
    this.zone.run(() => {
      console.error({
        message: error?.message,
        stack: error?.stack,
        error
      });
    });
  }
}
