import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { EditableMember, Member, Photo } from '../../types/member';
import { AccountService } from './account-service';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  public isEditMode = signal(false);
  public member = signal<Member | null>(null);

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'members');
    // return this.http.get<Member[]>(this.baseUrl + 'members', this.getHttpOptions());
  }

  getMember(id: string) {
    return this.http.get<Member>(this.baseUrl + 'members/' + id).pipe(
      tap(member => {
        this.member.set(member);
      })
    );
    //return this.http.get<Member>(this.baseUrl + 'members/' + id);
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

  updateMember(member: EditableMember) {
    return this.http.put(this.baseUrl + 'members', member);
  }
}
