import { UserService } from './../../../services/generated-api-code';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { base64ToFile, ImageCroppedEvent } from 'ngx-image-cropper';

@Component({
  selector: 'app-user-image-uploader',
  templateUrl: './user-image-uploader.component.html',
  styleUrls: ['./user-image-uploader.component.scss'],
})
export class UserImageUploaderComponent implements OnInit {
  @Input() data: any;

  imageChangedEvent: any = '';

  imageForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.imageForm = this.fb.group({
      userId: [this.data.userId, Validators.required],
      photo: ['', Validators.required],
    });
  }

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
  }

  imageCropped(event: ImageCroppedEvent): void {
    let croppedImage = base64ToFile(event.base64);
    this.imageForm.patchValue({ photo: croppedImage });
    this.imageForm.get('photo').updateValueAndValidity();
  }

  uploadImage(): void {
    this.userService
      .uploadPhoto(this.imageForm.get('userId').value, {
        fileName: 'photo',
        data: this.imageForm.get('photo').value,
      })
      .subscribe((path: string) => this.ngbModal.close(path));
  }

  close(): void {
    this.ngbModal.dismiss();
  }
}
