import { HttpClient } from '@angular/common/http';
import { TokenService } from 'src/app/services/token.service';
import { ModalService } from 'src/app/services/modal.service';
import { Component, Input, OnInit } from '@angular/core';
import { PictureEnlargerComponent } from '../picture-enlarger/picture-enlarger.component';
import { MedicalRecordService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-medical-record-displayer',
  templateUrl: './medical-record-displayer.component.html',
  styleUrls: ['./medical-record-displayer.component.scss'],
})
export class MedicalRecordDisplayerComponent implements OnInit {
  @Input() meds: any[];

  constructor(
    private modalService: ModalService,
    private tokenService: TokenService,
    private recordService: MedicalRecordService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {}

  getRole(): string {
    return this.tokenService.getUserData().role;
  }

  enlargeImage(photoUrl: string) {
    this.modalService.openModal(PictureEnlargerComponent, () => {}, { photoUrl: photoUrl }, 'lg');
  }

  generatePdf(medicalRecordId: string): void {
    this.http
      .get('https://localhost:5001/api/records/pdf/' + medicalRecordId, { responseType: 'blob' })
      .subscribe((response) => {
        var file = new Blob([response], { type: 'application/pdf' });
        var fileURL = URL.createObjectURL(file);
        window.open(fileURL);
      });
  }
}
