import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  // Signal to track the image availability state
  imageExists = signal<boolean>(false);
  private http = inject(HttpClient);

  // Method to check image availability
  async checkImageAvailable(imageUrl: string): Promise<string> {

    try {
      const response = await firstValueFrom(
        this.http.get("http://localhost:4200/src/" + imageUrl, { observe: 'response' })
      );

      this.imageExists.set(response.status === 200);
    } catch (error) {
      console.error('Error checking image availability:', error);
      this.imageExists.set(false);
    }

    return this.imageExists() ? imageUrl : 'assets/images/product-0.png';
  }
}
