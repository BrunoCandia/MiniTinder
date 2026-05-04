import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Member, Photo } from '../../../types/member';
import { AsyncPipe } from '@angular/common';
import { ImageUpload } from '../../../shared/image-upload/image-upload';
import { AccountService } from '../../../core/services/account-service';
import { User } from '../../../types/user';
import { StarButton } from "../../../shared/star-button/star-button";
import { DeleteButton } from "../../../shared/delete-button/delete-button";

@Component({
  selector: 'app-member-photos',
  imports: [AsyncPipe, ImageUpload, StarButton, DeleteButton],
  templateUrl: './member-photos.html',
  styleUrl: './member-photos.css',
})
export class MemberPhotos implements OnInit {
  protected memberService = inject(MemberService);
  private route = inject(ActivatedRoute);
  protected accountService = inject(AccountService);
  protected photos = signal<Photo[]>([]);
  //protected photos$?: Observable<Photo[]>;
  protected isLoading = signal(false);

  constructor() {
    // const memberId = this.route.parent?.snapshot.paramMap.get('id');

    // if (memberId) {
    //   this.memberService.getMemberPhotos(memberId).subscribe({
    //     next: photos => {
    //       this.photos.set(photos);
    //     }
    //   });
      
    //   //this.photos$ = this.memberService.getMemberPhotos(memberId);
    // }
  }
  
  ngOnInit(): void {
    const memberId = this.route.parent?.snapshot.paramMap.get('id');

    if (memberId) {
      this.memberService.getMemberPhotos(memberId).subscribe({
        next: photos => {
          this.photos.set(photos);
        }
      });            
    }
  }

  onUploadImage(file: File) {
    this.isLoading.set(true);
    this.memberService.uploadPhoto(file).subscribe({
      next: photo => {
        this.memberService.isEditMode.set(false);
        this.isLoading.set(false);
        this.photos.update(photos => [...photos, photo]);        
      },
      error: () => {
        console.error('Error uploading photo');
        this.isLoading.set(false);
      }
    });
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo).subscribe({
      next: () => {
        const currentUser = this.accountService.currentUser();
        if (currentUser) {
          currentUser.imageUrl = photo.url;
        }
        
        this.accountService.setCurrentUser(currentUser as User);
        this.memberService.member.update(member => ({
          ...member,
          imageUrl: photo.url
        }) as Member)
      },
      error: () => {
        console.error('Error setting main photo');
      }
    });
  }

  deletePhoto(photoId: number) {
    this.memberService.deletePhoto(photoId).subscribe({
      next: () => {
        this.photos.update(photos => photos.filter(p => p.id !== photoId));
      },
      error: () => {
        console.error('Error deleting photo');
      }
    });
  }
}