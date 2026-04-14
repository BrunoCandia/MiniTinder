import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member, Photo } from '../../types/member';
import { AccountService } from './account-service';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private accountService = inject(AccountService);

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'members');
    // return this.http.get<Member[]>(this.baseUrl + 'members', this.getHttpOptions());
  }

  getMember(id: string) {
    return this.http.get<Member>(this.baseUrl + 'members/' + id);
    // return this.http.get<Member>(this.baseUrl + 'members/' + id, this.getHttpOptions());
  }

  getMemberPhotos(id: string) {
    return this.http.get<Photo[]>(this.baseUrl + 'members/' + id + '/photos');
  }

  // private getHttpOptions() {
  //   return {
  //     headers: new HttpHeaders({
  //       Authorization: 'Bearer ' + this.accountService.currentUser()?.token
  //     })
  //   }
  // }
}
