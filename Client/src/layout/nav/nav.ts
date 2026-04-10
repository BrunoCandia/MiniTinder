import { Component, inject, signal} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  private toastService = inject(ToastService);
  private router = inject(Router);
  protected accountService = inject(AccountService);
  protected credentials: any = {};
  protected isLoggedIn = signal(false);

  login() {
    // console.log('Login with credentials:', this.credentials);
    this.accountService.login(this.credentials).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/members');
        this.toastService.showSuccess('Login successful!');
        this.credentials = {};
        //console.log('Login successful:', response);
        //this.isLoggedIn.set(true);
      },
      error: (error) => {
        console.error('Login failed:', error);
        this.toastService.showError(error.error);
        //this.isLoggedIn.set(false);
      }
    });
  }

  logout() {    
    this.accountService.logout();
    this.router.navigateByUrl('/');
    //console.log('Logged out');
    //this.isLoggedIn.set(false);
    // this.credentials = {};
  }
}
