import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-medical-record-displayer',
  templateUrl: './medical-record-displayer.component.html',
  styleUrls: ['./medical-record-displayer.component.scss'],
})
export class MedicalRecordDisplayerComponent implements OnInit {
  @Input() meds: any[];

  constructor() {}

  ngOnInit(): void {}

  enlargeImage(path): void {
    // TODO
  }
  generatePdf(medicalRecordId: string): void {
    // TODO
  }
}
