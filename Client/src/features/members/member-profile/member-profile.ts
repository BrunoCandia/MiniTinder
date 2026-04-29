import { Component, HostListener, inject, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EditableMember, Member } from '../../../types/member';
import { DatePipe } from '@angular/common';
import { MemberService } from '../../../core/services/member-service';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastService } from '../../../core/services/toast-service';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-member-profile',
  imports: [DatePipe, FormsModule],
  templateUrl: './member-profile.html',
  styleUrl: './member-profile.css',
})
export class MemberProfile implements OnInit, OnDestroy {
  
  @HostListener('window:beforeunload', ['$event']) notifyUser($event: BeforeUnloadEvent) {
    if (this.editForm?.dirty) {
      $event.preventDefault();
    }
  }

  @ViewChild('editForm') editForm?: NgForm;
  private accountService = inject(AccountService);
  protected memberService = inject(MemberService);
  private toastService = inject(ToastService);
  // private route = inject(ActivatedRoute);  
  // protected member = signal<Member | undefined>(undefined);
  // protected editableMember?: EditableMember;
  protected editableMember: EditableMember = {
    displayName: '',
    description: '',
    city: '',
    country: ''
  }
  
  // constructor() {
  //   this.editableMember = {
  //     displayName: this.member()?.displayName || '',
  //     description: this.member()?.description || '',
  //     city: this.member()?.city || '',
  //     country: this.member()?.country || ''
  //   }
  // }

  ngOnInit(): void {    
    // this.route.parent?.data.subscribe({
    //   next: (data) => {
    //     this.member.set(data['member']);
    //   }
    // });

    this.editableMember = {
      displayName: this.memberService.member()?.displayName || '',
      description: this.memberService.member()?.description || '',
      city: this.memberService.member()?.city || '',
      country: this.memberService.member()?.country || ''
    }
  }

  updateProfile(){
    if (!this.memberService.member()) return;

    const updatedMember = {...this.memberService.member(), ...this.editableMember};
    
    this.memberService.updateMember(this.editableMember).subscribe({
      next: () => {
        const currentUser = this.accountService.currentUser();
        if (currentUser && updatedMember.displayName !== currentUser?.displayName) {
          currentUser.displayName = updatedMember.displayName;
          this.accountService.setCurrentUser(currentUser);
        }

        this.toastService.showSuccess('Profile updated succesfully');
        this.memberService.isEditMode.set(false);
        this.memberService.member.set(updatedMember as Member);
        this.editForm?.reset(updatedMember);
      }
    });    
  }

  ngOnDestroy(): void {
    if (this.memberService.isEditMode()) {
      this.memberService.isEditMode.set(false);
    }
  }
}
