import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginCredentials, RegisterCredentials, User } from '../../types/user';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);  

  register(credentials: RegisterCredentials) {
    return this.http.post<User>(this.baseUrl + 'account/register', credentials).pipe(
      tap(user => {
        if(user) {
          this.setCurrentUser(user);
          // localStorage.setItem('user', JSON.stringify(user));
          // this.currentUser.set(user);
        }
      })
    );
  }

  login(credentials: LoginCredentials) {
    return this.http.post<User>(this.baseUrl + 'account/login', credentials).pipe(
      tap(user => {
        if(user) {
          this.setCurrentUser(user);
          // localStorage.setItem('user', JSON.stringify(user));
          // this.currentUser.set(user);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }
}
