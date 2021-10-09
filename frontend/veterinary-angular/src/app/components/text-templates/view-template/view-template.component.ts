import { MedicalRecordTextTemplate } from './../../../services/generated-api-code';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-view-template',
  templateUrl: './view-template.component.html',
  styleUrls: ['./view-template.component.scss'],
})
export class ViewTemplateComponent implements OnInit {
  @Input() data: any;

  template: MedicalRecordTextTemplate;

  constructor(private ngbModal: NgbActiveModal) {}

  ngOnInit(): void {
    this.template = this.data.textTemplate;
  }

  close(): void {
    this.ngbModal.dismiss();
  }
}
