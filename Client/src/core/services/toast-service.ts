import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor() { 
    this.createToastContainer();
  }

  private createToastElement(message: string, alertClass: string, duration = 5000) {
    const toastContainer = document.getElementById('toast-container');
    
    if (!toastContainer) return;
    
    const toast = document.createElement('div');
    toast.classList.add('alert', alertClass, 'shadow-lg');
    toast.innerHTML = `      
        <span>${message}</span>
        <button class="ml-4 btn btn-sm btn-ghost" onclick="this.parentElement.remove()">×</button>      
    `;

    toast.querySelector('button')?.addEventListener('click', () => {
      toastContainer.removeChild(toast);
    });

    toastContainer.appendChild(toast);

    setTimeout(() => {
      if (toastContainer.contains(toast)) {
        toastContainer.removeChild(toast);
      }
    }, duration);
  }

  private createToastContainer() {
    if (!document.getElementById('toast-container')) {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end';
      document.body.appendChild(container);
    }
  }

  showSuccess(message: string, duration?: number) {
    this.createToastElement(message, 'alert-success', duration);
  }

  showError(message: string, duration?: number) {
    this.createToastElement(message, 'alert-error', duration);
  }

  showWarning(message: string, duration?: number) {
    this.createToastElement(message, 'alert-warning', duration);
  }

  showInfo(message: string, duration?: number) {
    this.createToastElement(message, 'alert-info', duration);
  }
}
