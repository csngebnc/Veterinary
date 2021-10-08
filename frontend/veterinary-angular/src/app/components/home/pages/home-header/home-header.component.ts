import { UserService } from './../../../../services/generated-api-code';
import { UserImageUploaderComponent } from './../../../_partials/user-image-uploader/user-image-uploader.component';
import { ModalService } from './../../../../services/modal.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { UserData } from 'src/app/services/token.service';

@Component({
  selector: 'app-home-header',
  templateUrl: './home-header.component.html',
  styleUrls: ['./home-header.component.scss'],
})
export class HomeHeaderComponent implements OnInit {
  @Input() userInfo: UserData;

  notesFormControl: FormControl;
  photoUrl: string;

  constructor(private modalService: ModalService, private userService: UserService) {}

  ngOnInit(): void {
    this.userService
      .getPhotoUrl(this.userInfo.id)
      .subscribe((path: string) => (this.photoUrl = path));
    this.notesFormControl = new FormControl(localStorage.getItem(`note-${this.userInfo.id}`));
  }

  editPhoto() {
    this.modalService.openModal(
      UserImageUploaderComponent,
      (path: string) => (this.photoUrl = path),
      {
        userId: this.userInfo.id,
        photoUrl: this.userInfo.photoUrl,
      }
    );
  }

  deletePhoto() {
    this.userService
      .deletePhoto(this.userInfo.id)
      .subscribe((path: string) => (this.photoUrl = path));
  }

  saveNotes() {
    if (this.notesFormControl.value.trim().length === 0) {
      localStorage.removeItem('note-' + this.userInfo.id);
    } else {
      localStorage.setItem('note-' + this.userInfo.id, this.notesFormControl.value);
    }
  }
}
