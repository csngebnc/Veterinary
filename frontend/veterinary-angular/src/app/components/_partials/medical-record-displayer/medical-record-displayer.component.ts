import { TokenService } from 'src/app/services/token.service';
import { ModalService } from 'src/app/services/modal.service';
import { Component, Input, OnInit } from '@angular/core';
import { PictureEnlargerComponent } from '../picture-enlarger/picture-enlarger.component';

@Component({
  selector: 'app-medical-record-displayer',
  templateUrl: './medical-record-displayer.component.html',
  styleUrls: ['./medical-record-displayer.component.scss'],
})
export class MedicalRecordDisplayerComponent implements OnInit {
  @Input() meds: any[];

  constructor(private modalService: ModalService, private tokenService: TokenService) {}

  ngOnInit(): void {}

  getRole(): string {
    return this.tokenService.getUserData().role;
  }

  enlargeImage(photoUrl: string) {
    this.modalService.openModal(PictureEnlargerComponent, () => {}, { photoUrl: photoUrl }, 'lg');
  }

  generatePdf(medicalRecordId: string): void {
    // TODO
  }
}
