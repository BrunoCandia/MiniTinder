import { Component, inject, signal} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountService = inject(AccountService);
  protected credentials: any = {};
  protected isLoggedIn = signal(false);

  login() {
    // console.log('Login with credentials:', this.credentials);
    this.accountService.login(this.credentials).subscribe({
      next: (response) => {
        console.log('Login successful:', response);
        //this.isLoggedIn.set(true);
        this.credentials = {};
      },
      error: (error) => {
        console.error('Login failed:', error);
        //this.isLoggedIn.set(false);
      }
    });
  }

  logout() {
    console.log('Logged out');
    this.accountService.logout();
    //this.isLoggedIn.set(false);
    // this.credentials = {};
  }
}
