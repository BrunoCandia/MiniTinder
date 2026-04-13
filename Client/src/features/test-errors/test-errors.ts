import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css',
})
export class TestErrors {
  private http = inject(HttpClient);
  baseUrl = 'https://localhost:5001/api/';
  validationErrors = signal<string[]>([]);

  getNotFoundError() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  getBadRequestError() {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  getInternalServerError() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  getAuthorizationError() {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  getBadRequestValidationError() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.error(error);
        this.validationErrors.set(error);
      }
    });
  }
}
