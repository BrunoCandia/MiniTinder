import { Component, inject, input, output } from '@angular/core';
import { RegisterCredentials, User } from '../../../types/user';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private accountService = inject(AccountService);
  membersFromHome = input.required<User[]>();
  cancelRegister = output<boolean>();
  protected crdentials = {} as RegisterCredentials;

  register() {
    console.log('Registering with credentials:', this.crdentials);
    this.accountService.register(this.crdentials).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        this.cancel();
      },
      error: (error) => {
        console.error('Registration failed:', error);
      }
    });
  }

  cancel() {
    console.log('Registration cancelled');
    this.cancelRegister.emit(false);
  }
}
