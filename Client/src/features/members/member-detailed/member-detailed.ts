import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { Member } from '../../../types/member';
import { Observable } from 'rxjs/internal/Observable';
import { filter } from 'rxjs';
import { AgePipe } from '../../../core/pipes/age-pipe';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-member-detailed',
  imports: [AsyncPipe, RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  templateUrl: './member-detailed.html',
  styleUrl: './member-detailed.css',
})
export class MemberDetailed implements OnInit {
  protected memberService = inject(MemberService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private accountService = inject(AccountService);   
  //protected member = signal<Member | undefined>(undefined);
  //protected member$?: Observable<Member>;
  protected title = signal<string | undefined>('Profile');

  protected isCurrentUser = computed(() => {
    const currentUser = this.accountService.currentUser()?.id === this.route.snapshot.paramMap.get('id');
    return currentUser;
  });

   ngOnInit(): void {
     //this.member$ = this.loadMember();

    // this.route.data.subscribe({
    //   next: (data) => {
    //     this.member.set(data['member']);
    //   }
    // });

     this.title.set(this.route.firstChild?.snapshot?.title);

     this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
     ).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot?.title);
      }
     });
   }

  //  loadMember() {
  //   const id = this.route.snapshot.paramMap.get('id');

  //   if (!id) {
  //     return;
  //   }
    
  //   return this.memberService.getMember(id);    
  //  }
}
