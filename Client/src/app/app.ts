import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('Mini Tinder');
  protected members = signal<any>([]);

  constructor(private http: HttpClient) {}

  //Using Subscriptions
  // ngOnInit(): void {
  //   this.http.get('https://localhost:5001/api/members').subscribe({
  //     next: (response) => {
  //       console.log(response);
  //       this.members.set(response);
  //     },
  //     error: (error) => {
  //       console.error(error);
  //     },
  //     complete: () => {
  //       console.log('Request completed');
  //     }
  //   });
  // }

  // Using Promises
  async ngOnInit(): Promise<void> {
    this.members.set(await this.getMembers());
  }
  
  async getMembers() {
    try {
      return lastValueFrom(this.http.get('https://localhost:5001/api/members'));
    } catch (error) {
      console.error(error);
      throw error;
    }    
  }
}
